using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static StageLoader;
using static UnityEngine.EventSystems.EventTrigger;


public enum StoryMode
{
    None,
    Tutorial,
    Story,
}



public class TextStoryManager : MonoBehaviour
{
    UIManager uiManager;

    private StoryLayout storyLayout;

    Sprite currentCharacter,currentImage,currentTransition;
    AudioClip currentBGM;

    [SerializeField] TextFadeIn textFadeIn; 

    public int storyNum = 0; // ストーリーの進捗度。テキストのまとまりを管理する。
    public int textPage = 1; // テキストの進捗度。１ストーリー内におけるテキストの進捗を管理する。StoryCountが変動すると初期値に戻る。

    public bool isTextClicked = false; // テキスト表示時にクリックが押されたかどうか（次のページに進むための行動がされたか）を計る。
                                       // ToDo => テキストを一文字ずつ出す場合、テキストが全部出たかどうかのboolを取り、falseならクリックによって全テキストが表示されてtrueに、trueならクリックされたときに次ページに行くようにする。


    // UI系統のゲームオブジェクトのアタッチ
    [SerializeField] GameObject canvas;
    [SerializeField] Image charaIconObj;
    [SerializeField] TextMeshProUGUI storyTextObj;
    [SerializeField] Image transitionImageObj;
    [SerializeField] Image backgroundImageObj;
    BGManager bgManager;


    // 使用する全リソース（素材）のアタッチをするリスト
    [SerializeField] Sprite[] Characters;
    [SerializeField] Sprite[] Transitions;
    [SerializeField] Sprite[] Images;
    [SerializeField] AudioClip[] BGMs;


    [System.Serializable]
    public class StoryLayouts
    {
        public int TextNum;
        public string Character;
        public string Text;
        public string Transition;
        public string Image;
        public string BGM;
    }

    [System.Serializable]
    public class StoryLayout
    {
        public StoryLayouts[] layout;
    }


    // イベントドリブン
    public event Action<StoryMode> StoryModeChanged;

    StoryMode _storyMode;

    public StoryMode storyMode
    {
        get => _storyMode;
        set
        {
            if (_storyMode == value) return;
            _storyMode = value;
            StoryModeChanged?.Invoke(_storyMode);
        }
    }

    [SerializeField] private InputAction clickAction;
    private void OnEnable()
    {
        clickAction.performed += OnClick;
        clickAction.Enable();
    }

    private void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }




    void Awake()
    {
        DontDestroyOnLoad(this);

        uiManager = FindAnyObjectByType<UIManager>();
        bgManager = FindAnyObjectByType<BGManager>();

        canvas.SetActive(false);
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("StoryNum")) PlayerPrefs.SetInt("StoryNum", 0); // ストーリーの進捗がnullならStoryNum=0で初期化（チュートリアルからスタートになる）
        if (!PlayerPrefs.HasKey("TextPage")) PlayerPrefs.SetInt("TextPage", 1); // テキストは１
    }


    public void LoadStory()
    {
        // 最初にstoryNumとtextPageをPlayerPrefsから受け取る。これによって、どのストーリーのどのテキストを表示するかが決まる。
        storyNum = PlayerPrefs.GetInt("StoryNum");
        textPage = PlayerPrefs.GetInt("TextNum");

        // 次にstoryNumに応じたストーリー情報が入っているJSONファイルを読み込む。
        TextAsset jsonFile = Resources.Load<TextAsset>($"StoryTexts/Story_{storyNum}");
        storyLayout = JsonUtility.FromJson<StoryLayout>(jsonFile.text);


        // そしてテキスト・キャラクター・テキストなどの情報をTextNumに従って受け取る処理を動かす。
        canvas.SetActive(true);
        StoryCycle(storyLayout);
    }



    void StoryCycle(StoryLayout storyLayout)
    {
        // まずはリストとなるentriesに、textPage = JSONのTextNumとなる行の(1 * N)のStoryLayoutsデータをいれる。
        var entries = storyLayout.layout
                   .Where(p => p != null && p.TextNum == textPage)
                   .ToList();


        // 空白のデータ行が含まれていた際の再帰処理
        if(entries.Count == 0)
        {
            if(storyLayout.layout.Any(p => p != null && p.TextNum > textPage))
            {
                textPage++;
                StoryCycle(storyLayout);
            }
        }


        // 最後のTextNumかどうかを判断し、結果に応じて処理を変えられるようにDisplay～関数にboolを引き渡す。
        if(storyLayout.layout.Any(p => p != null && p.TextNum > textPage))
        {
            List<Coroutine> coroutines = new List<Coroutine>();
            foreach (var entry in entries)
                coroutines.Add(StartCoroutine(DisplayStoryLayouts(entry, false)));
        }
        else
        {
            List<Coroutine> coroutines = new List<Coroutine>();
            foreach (var entry in entries)
                coroutines.Add(StartCoroutine(DisplayStoryLayouts(entry, true)));
        }
        
    }



    IEnumerator DisplayStoryLayouts(StoryLayouts entry, bool IsEndOfStory)
    {
        // 受け取った情報の種類ごとに変数を用意し格納。
        string character = entry.Character;
        string text = entry.Text;
        string transition = entry.Transition;
        string image = entry.Image;
        string bgm = entry.BGM;


        // 次にImage画像系はリストの中から今回の格納データと名前一致するものを取得する。
        //　ToDo => 名前一致だと「…_0」とかで一致しなくなる可能性があるため現在は一部一致にしている。だが、表情差分を入れる際にこれだと正しく表示できない為、必要になったら直す。
        foreach (var c in Characters)
        {
            if (c.name.Contains(character)) currentCharacter = c;
        } 

        foreach (var t in Transitions)
        {
            if (t.name.Contains(transition)) currentTransition = t;
        }

        foreach (var i in Images)
        {
            if (i.name.Contains(image)) currentImage = i;
        }

        foreach (var b in BGMs)
        {
            if (b.name.Contains(bgm)) currentBGM = b;
        }


        // そして各オブジェクトのSpriteやテキストを変更する。
        // ToDo BGMはBGManagerに直接受け取った内容を受け渡す形になる。UIManagerのSceneEventの状態（T or F）に応じてBGM変化方法を変えた後、AudioClipを受け取る変数をBGManager内に作るなど。
        
        charaIconObj.sprite = currentCharacter;

        storyTextObj.text = text;

         
        textFadeIn.StartCoroutine(textFadeIn.FadeIn());

        transitionImageObj.sprite = currentTransition;

        if(currentImage != null)
        {
            backgroundImageObj.sprite = currentImage;
        }
        


        // クリックされるまで次ページには進まず、待機する。
        yield return new WaitUntil(() => isTextClicked);

        isTextClicked = false;
        
        if(IsEndOfStory)
        {
            // 終了処理を書く。（トランジション方法やらBGMの変更やら）


            // 格納用の変数やらローカル変数を初期化（ = null）
            currentCharacter = null;
            currentTransition = null;
            currentImage = null;
            currentBGM = null;

            character = null;
            text = null;
            transition = null;
            image = null;
            bgm = null;


            // ストーリー進捗を上昇(UpdateStoryNum())させ、ストーリーモードを終了する。
            UpdateStoryNum();
            canvas.SetActive(false);
            storyMode = StoryMode.None;
        }
        else
        {
            if(textFadeIn.isAllTextAppeared)
            {
                // 最終ページでないならページを次に移行し、ストーリーのサイクルを回す。
                textPage++;
                StoryCycle(storyLayout);
            }
            else
            {
                // テキストがすべて表示されていない場合は、テキストをすべて表示させる。
                textFadeIn.skipRequest = true;

                yield return new WaitUntil(() => isTextClicked);

                isTextClicked = false;

                textPage++;
                StoryCycle(storyLayout);
            }
            
        }

        yield return null;
    }

    public void OnClick(InputAction.CallbackContext ctx) // ToDo => クリックの入力が受け付けられていない。要確認。
    {
        if (!ctx.started) return;

        Debug.Log("クリック検知");

        if(storyMode != StoryMode.None && isTextClicked == false)
        {
            isTextClicked = true;
        }
    }



    public void UpdateStoryNum()
    {
        // ストーリーの進捗は＋１
        storyNum = PlayerPrefs.GetInt("StoryNum") + 1;
        PlayerPrefs.SetInt("StoryNum", storyNum);
        PlayerPrefs.Save();

        // テキストページは初期化（1になる）
        textPage = 0;
        PlayerPrefs.SetInt("TextPage", 1);
        PlayerPrefs.Save();
    }
}
