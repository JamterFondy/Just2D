using UnityEngine;
using UnityEngine.UI;

public class FromBegin : MonoBehaviour
{
    UIManager uiManager;
    ButtonSE buttonSE;


    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        buttonSE = FindAnyObjectByType<ButtonSE>();
    }

    
    public void OnClick()
    {
        uiManager.currentState = UIState.GameStartConfirm;
        buttonSE.PlayButtonSE("Confirm");
    }
}
