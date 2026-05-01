using UnityEngine;
using UnityEngine.UI;

public class SettingsESC : MonoBehaviour
{
    UIManager uiManager;
    TabSettings tabSettings;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        tabSettings = FindObjectOfType<TabSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }

    public void OnClick() //ToDo⇒SceneTypeがTitleやBattleなど、tabSettingsの参照を取れないときに対応できるようにする。（SceneTypeで場合分け？)
    {
        if(tabSettings == null) tabSettings = FindObjectOfType<TabSettings>();
        
        if(uiManager.currentScene == SceneType.Title) uiManager.currentState = UIState.TitleDefault;
        else uiManager.currentState = tabSettings.preState;
    }
}
