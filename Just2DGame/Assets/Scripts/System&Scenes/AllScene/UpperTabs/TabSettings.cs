using UnityEngine;

public class TabSettings : MonoBehaviour
{
    public UIState preState;

    public void OnClick()
    {
        if (UIManager.Instance.currentState != UIState.Settings)
        {
            preState = UIManager.Instance.currentState;
            UIManager.Instance.currentState = UIState.Settings;
            SEManager.Instance.PlaySE("Button", "Confirm_Button", "Confirm");
        }
        else
        {
            UIManager.Instance.currentState = preState;
            SEManager.Instance.PlaySE("Button", "Cancel_Button", "Cancel");
        }
    }

}
