using UnityEngine;
using System;

public class CancelButton : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.currentState = UIState.TitleDefault;
        SEManager.Instance.PlaySE("Button", "Cancel_Button", "Cancel");
    }
}
