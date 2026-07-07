using UnityEngine;

public class PlayerPrefsEventManager : MonoBehaviour
{
    // PlayerPrefsの値に応じてイベントの発火をつかさどるクラス。
    // シーン別に起きうるイベントをPlayerPrefsの値と照らし合わせてシーン遷移が行われたタイミングで呼び出されるようにする。
    // 例) ホームシーンではStoryNum = 0,3,6...でストーリーが、StoryNum = 1,4,5...でチュートリアルのイベントがある。
    // シーン遷移かUIStateなどの状態変遷をトリガーにイベントごとの関数を呼び出す。

    UIManager uiManager;
    TextStoryManager textStoryManager;


    private void Awake()
    {
        textStoryManager = FindAnyObjectByType<TextStoryManager>();


        UIManager.Instance.SceneChanged += OnSceneChanged;
    }

    void OnSceneChanged(SceneType scene) => StartStoryEvent(scene);

    void StartStoryEvent(SceneType scene)
    {
        if(scene == SceneType.Home)
        {
            if (PlayerPrefs.GetInt("StoryNum") == 0)
            {
                textStoryManager.storyMode = StoryMode.Story;
                textStoryManager.LoadStory();
            }
        }
        
    }

}
