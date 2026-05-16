using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using static StageLoader;
using static UnityEngine.EventSystems.EventTrigger;


public class TextStoryManager : MonoBehaviour
{
    UIManager uiManager;

    private StoryLayout storyLayout;

    Sprite currentCharacter, currentTransition, currentImage;
    AudioClip currentBGM;

    public int storyNum = 0; // ストーリーの進捗度。テキストのまとまりを管理する。
    public int textPage = 1; // テキストの進捗度。１ストーリー内におけるテキストの進捗を管理する。StoryCountが変動すると初期値に戻る。

    // UI系統のゲームオブジェクトのアタッチ
    [SerializeField] SpriteRenderer charaIconObj;
    [SerializeField] TextMeshProUGUI storyTextObj;
    [SerializeField] SpriteRenderer transitionImageObj;
    [SerializeField] SpriteRenderer backgroundImageObj;
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


    void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        bgManager = FindAnyObjectByType<BGManager>();
    }



    void Start()
    {
        if (!PlayerPrefs.HasKey("StoryNukm")) PlayerPrefs.SetInt("StoryNum", 0); // ストーリーの進捗がnullならStoryNum=0で初期化（チュートリアルからスタートになる）
        if (!PlayerPrefs.HasKey("TextPage")) PlayerPrefs.SetInt("TextPage", 1); // テキストは１

        storyNum = PlayerPrefs.GetInt("StoryNum");
        textPage = PlayerPrefs.GetInt("TextNum");
    }



    public void StoryMode(bool IsStoryMode)
    {
        if (IsStoryMode) LoadStory();
    }



    void LoadStory()
    {
        // まずstoryNumに応じたストーリー情報が入っているJSONファイルを読み込む。
        TextAsset jsonFile = Resources.Load<TextAsset>($"StoryTexts/Story_{storyNum}");
        storyLayout = JsonUtility.FromJson<StoryLayout>(jsonFile.text);


        // 次にテキスト・キャラクター・テキストなどの情報をTextNumに従って受け取る処理を動かす。
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
        foreach (var c in Characters)
        {
            if (c.name == character) currentCharacter = c;
        } 

        foreach (var t in Transitions)
        {
            if (t.name == transition) currentTransition = t;
        }

        foreach (var i in Images)
        {
            if (i.name == image) currentImage = i;
        }

        foreach (var b in BGMs)
        {
            if (b.name == bgm) currentBGM = b;
        }


        // そして各オブジェクトのSpriteやテキストを変更する。
        // ToDo BGMはBGManagerに直接受け取った内容を受け渡す形になる。UIManagerのSceneEventの状態（T or F）に応じてBGM変化方法を変えた後、AudioClipを受け取る変数をBGManager内に作るなど。
        charaIconObj.sprite = currentCharacter;
        storyTextObj.text = text;
        transitionImageObj.sprite = currentTransition;
        backgroundImageObj.sprite = currentImage;


        yield return null;
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
