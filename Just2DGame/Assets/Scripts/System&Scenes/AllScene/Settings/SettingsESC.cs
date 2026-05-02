using UnityEngine;
using UnityEngine.UI;

public class SettingsESC : MonoBehaviour
{
    UIManager uiManager;
    TabSettings tabSettings;
    ButtonSE buttonSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        tabSettings = FindAnyObjectByType<TabSettings>();
        buttonSE = FindAnyObjectByType<ButtonSE>();
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
        if(tabSettings == null) tabSettings = FindAnyObjectByType<TabSettings>();
        
        if(uiManager.currentScene == SceneType.Title) uiManager.currentState = UIState.TitleDefault;
        else uiManager.currentState = tabSettings.preState;

        buttonSE.PlayButtonSE("Cancel");
    }
}
