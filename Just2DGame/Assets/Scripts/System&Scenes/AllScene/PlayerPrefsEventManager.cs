using UnityEngine;

public class PlayerPrefsEventManager : MonoBehaviour
{
    // PlayerPrefsの値に応じてイベントの発火をつかさどるクラス。
    // シーン別に起きうるイベントをPlayerPrefsの値と照らし合わせてシーン遷移が行われたタイミングで呼び出されるようにする。
    // 例) ホームシーンではStoryNum = 0,3,6...でストーリーが、StoryNum = 1,4,5...でチュートリアルのイベントがある。
    // シーン遷移かUIStateなどの状態変遷をトリガーにイベントごとの関数を呼び出す。
    public void StartStoryEvent()
    {

    }
}
