using UnityEngine;
using UnityEngine.UI;

public class SettingsESC : MonoBehaviour
{
    UIManager uiManager;
    UpperTabsUIs upperTabsUIs;
    SEManager seManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        upperTabsUIs = FindAnyObjectByType<UpperTabsUIs>();
        seManager = FindAnyObjectByType<SEManager>();
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
        if(upperTabsUIs == null) upperTabsUIs = FindAnyObjectByType<UpperTabsUIs>();

        if (uiManager.currentScene == SceneType.Title) uiManager.currentState = UIState.TitleDefault;
        else if (uiManager.currentScene == SceneType.Battle) uiManager.currentState = UIState.BattlePauseMenu;
        else uiManager.currentState = upperTabsUIs.preState;


        seManager.PlaySE("Button", "Cancel_Button", "Cancel");
    }
}
