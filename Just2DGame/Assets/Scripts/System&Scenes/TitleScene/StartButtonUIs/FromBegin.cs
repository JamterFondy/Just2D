using UnityEngine;
using UnityEngine.UI;

public class FromBegin : MonoBehaviour
{
    UIManager uiManager;

    
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    
    public void OnClick()
    {
        uiManager.currentState = UIState.GameStartConfirm;
    }
}
