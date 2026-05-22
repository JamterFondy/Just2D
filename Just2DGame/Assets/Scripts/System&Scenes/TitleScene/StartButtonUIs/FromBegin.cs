using UnityEngine;
using UnityEngine.UI;

public class FromBegin : MonoBehaviour
{
    UIManager uiManager;
    SEManager seManager;


    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        seManager = FindAnyObjectByType<SEManager>();
    }

    
    public void OnClick()
    {
        uiManager.currentState = UIState.GameStartConfirm;
        seManager.PlaySE("Button", "Confirm_Button", "Confirm");
    }
}
