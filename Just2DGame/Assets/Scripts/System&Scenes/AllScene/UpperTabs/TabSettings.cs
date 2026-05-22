using UnityEngine;

public class TabSettings : MonoBehaviour
{
    UIManager uiManager;
    public UIState preState;
    SEManager seManager;

    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        seManager = FindAnyObjectByType<SEManager>();
    }

    public void OnClick()
    {
        if (uiManager.currentState != UIState.Settings)
        {
            preState = uiManager.currentState;
            uiManager.currentState = UIState.Settings;
            seManager.PlaySE("Button", "Confirm_Button", "Confirm");
        }
        else
        {
            uiManager.currentState = preState;
            seManager.PlaySE("Button", "Cancel_Button", "Cancel");
        }
    }

}
