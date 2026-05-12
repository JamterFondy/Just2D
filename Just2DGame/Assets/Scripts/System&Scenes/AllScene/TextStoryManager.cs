using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class TextStoryManager : MonoBehaviour
{
    UIManager uiManager;

    public int StoryCount = 0; //ストーリーの進捗度。テキストのまとまりを管理する。
    public int TextPage = 1; //テキストの進捗度。１ストーリー内におけるテキストの進捗を管理する。StoryCountが変動すると初期値に戻る。


    [System.Serializable]
    public class Story
    {
        public int TextNum;
        public string Character;
        public string Text;
        public string Transition;
        public string Image;
        public string BGM;
    }


    void Awake()
    {
        uiManager = FindAnyObjectByType<UIManager>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
