using UnityEngine;

public class TextStoryManager : MonoBehaviour
{
    UIManager uiManager;

    public int StoryCount = 0; //ストーリーの進捗度。テキストのまとまりを管理する。
    public int TextPage = 1; //テキストの進捗度。１ストーリー内におけるテキストの進捗を管理する。StoryCountが変動すると初期値に戻る。

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
