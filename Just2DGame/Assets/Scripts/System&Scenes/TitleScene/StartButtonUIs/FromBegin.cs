using UnityEngine;
using UnityEngine.UI;

public class FromBegin : MonoBehaviour
{   
    public void OnClick()
    {
        UIManager.Instance.currentState = UIState.GameStartConfirm;
        SEManager.Instance.PlaySE("Button", "Confirm_Button", "Confirm");
    }
}
