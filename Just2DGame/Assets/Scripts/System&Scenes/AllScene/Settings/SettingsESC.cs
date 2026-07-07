using UnityEngine;
using UnityEngine.UI;

public class SettingsESC : MonoBehaviour
{
    UpperTabsUIs upperTabsUIs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        upperTabsUIs = FindAnyObjectByType<UpperTabsUIs>();
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

        if (UIManager.Instance.currentScene == SceneType.Title) UIManager.Instance.currentState = UIState.TitleDefault;
        else if (UIManager.Instance.currentScene == SceneType.Battle) UIManager.Instance.currentState = UIState.BattlePauseMenu;
        else UIManager.Instance.currentState = upperTabsUIs.preState;


        SEManager.Instance.PlaySE("Button", "Cancel_Button", "Cancel");
    }
}
