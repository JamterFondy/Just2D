using UnityEngine;

public class TabSettings : MonoBehaviour
{
    UIManager uiManager;
    public UIState preState;
    
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void OnClick()
    {
        if (uiManager.currentState != UIState.Settings)
        {
            preState = uiManager.currentState;
            uiManager.currentState = UIState.Settings;
        }
        else
        {
            uiManager.currentState = preState;
        }
    }

}
