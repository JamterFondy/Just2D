using UnityEngine;

public class TabSettings : MonoBehaviour
{
    UIManager uiManager;
    public UIState preState;
    ButtonSE buttonSE;

    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        buttonSE = FindAnyObjectByType<ButtonSE>();
    }

    public void OnClick()
    {
        if (uiManager.currentState != UIState.Settings)
        {
            preState = uiManager.currentState;
            uiManager.currentState = UIState.Settings;
            buttonSE.PlayButtonSE("Confirm");
        }
        else
        {
            uiManager.currentState = preState;
            buttonSE.PlayButtonSE("Cancel");
        }
    }

}
