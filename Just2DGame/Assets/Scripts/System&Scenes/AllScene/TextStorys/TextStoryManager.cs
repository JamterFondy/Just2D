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

    Sprite currentCharacter,currentBackground,currentTransition;
    AudioClip currentBGM;

    [SerializeField] TextFadeIn textFadeIn; 

    public int storyNum = 0; // ストーリーの進捗度。テキストのまとまりを管理する。
    public int textPage = 1; // テキストの進捗度。１ストーリー内におけるテキストの進捗を管理する。StoryCountが変動すると初期値に戻る。

    public bool isTextClicked = false; // テキスト表示時にクリックが押されたかどうか（次のページに進むための行動がされたか）を計る。
                                       

    // UI系統のゲームオブジェクトのアタッチ
    [SerializeField] GameObject canvas;
    [SerializeField] Image charaIconObj;
    [SerializeField] TextMeshProUGUI storyTextObj;
    [SerializeField] Image transitionImageObj;
    [SerializeField] Image backgroundImageObj;
    BGManager bgManager;


    // 使用する全リソース（素材）のアタッチをするリスト
    [SerializeField] Sprite[] Characters_sprites;
    [SerializeField] Sprite[] Transitions_sprites;
    [SerializeField] Sprite[] Background_sprites;
    [SerializeField] AudioClip[] BGMs_audioClips;


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
        // まずはリストとなるentriesに、textPage = (JSONのTextNum)となる行の(1 * N)のStoryLayoutsデータをいれる。
        var currentLineDatas_Json = storyLayout.layout
                   .Where(column => column != null && column.TextNum == textPage)
                   .ToList();


        // 空白のデータ行が含まれていた際の再帰処理
        if(currentLineDatas_Json.Count == 0)
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
            foreach (var JsonDatas in currentLineDatas_Json)
                coroutines.Add(StartCoroutine(DisplayStoryLayouts(JsonDatas, false)));
        }
        else
        {
            List<Coroutine> coroutines = new List<Coroutine>();
            foreach (var JsonDatas in currentLineDatas_Json)
                coroutines.Add(StartCoroutine(DisplayStoryLayouts(JsonDatas, true)));
        }
        
    }



    IEnumerator DisplayStoryLayouts(StoryLayouts JsonDatas, bool IsEndOfStory)
    {
        // 受け取った情報の種類ごとの変数を用意し格納。
        string character_receiver = JsonDatas.Character;
        string text_receiver = JsonDatas.Text;
        string transition_receiver = JsonDatas.Transition;
        string background_receiver = JsonDatas.Image;
        string bgm_receiver = JsonDatas.BGM;

        // 次にImage画像系はリストの中から今回の格納データと名前一致するものを取得する。
        //　ToDo => 名前一致だと「…_0」とかで一致しなくなる可能性があるため現在は一部一致にしている。だが、表情差分を入れる際にこれだと正しく表示できない為、必要になったら直す。
        foreach (var c in Characters_sprites)
        {
            if (c.name.Contains(character_receiver)) currentCharacter = c;
        } 

        foreach (var t in Transitions_sprites)
        {
            if (t.name.Contains(transition_receiver)) currentTransition = t;
        }

        foreach (var i in Background_sprites)
        {
            if (i.name.Contains(background_receiver)) currentBackground = i;
        }

        foreach (var b in BGMs_audioClips)
        {
            if (b.name.Contains(bgm_receiver)) currentBGM = b;
        }


        // そして各オブジェクトのSpriteやテキストを変更する。
        // ToDo BGMはBGManagerに直接受け取った内容を受け渡す形になる。UIManagerのSceneEventの状態（T or F）に応じてBGM変化方法を変えた後、AudioClipを受け取る変数をBGManager内に作るなど。
        
        charaIconObj.sprite = currentCharacter;

        storyTextObj.text = text_receiver;
        
        
        textFadeIn.StartCoroutine(textFadeIn.FadeIn());

        transitionImageObj.sprite = currentTransition;

        if(currentBackground != null)
        {
            backgroundImageObj.sprite = currentBackground;
        }
        


        // クリックされるまで次ページには進まず、待機する。
        yield return new WaitUntil(() => isTextClicked);

        isTextClicked = false;
        
        if(IsEndOfStory)
        {
            // 終了処理を書く。（トランジション方法やらBGMの変更やら）
            //ただし、テキストがすべて表示されているかどうかで処理を分けること。

            if (textFadeIn.isAllTextAppeared)
            {
                // 格納用の変数やらローカル変数を初期化（ = null）
                currentCharacter = null;
                currentTransition = null;
                currentBackground = null;
                currentBGM = null;

                character_receiver = null;
                text_receiver = null;
                transition_receiver = null;
                background_receiver = null;
                bgm_receiver = null;

                // ストーリー進捗を上昇(UpdateStoryNum())させ、ストーリーモードを終了する。
                UpdateStoryNum();
                canvas.SetActive(false);
                storyMode = StoryMode.None;
            }
            else
            {
                // テキストがすべて表示されていない場合は、テキストをすべて表示させる。その後、クリックされたらストーリーの終了処理に入る。
                textFadeIn.skipRequest = true;

                yield return new WaitUntil(() => isTextClicked);

                isTextClicked = false;


                // 格納用の変数やらローカル変数を初期化（ = null）
                currentCharacter = null;
                currentTransition = null;
                currentBackground = null;
                currentBGM = null;

                character_receiver = null;
                text_receiver = null;
                transition_receiver = null;
                background_receiver = null;
                bgm_receiver = null;


                // ストーリー進捗を上昇(UpdateStoryNum())させ、ストーリーモードを終了する。
                UpdateStoryNum();
                canvas.SetActive(false);
                storyMode = StoryMode.None;
            }

        }
        else
        {
            // 最終ページでないならページを次に移行し、ストーリーのサイクルを回す。

            if (textFadeIn.isAllTextAppeared)
            {
                // テキストがすべて表示されているならストーリーのサイクルを回す。
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

    public void OnClick(InputAction.CallbackContext ctx)
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
