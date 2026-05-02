using UnityEngine;
using System;

public class CancelButton : MonoBehaviour
{
    UIManager uiManager;
    ButtonSE buttonSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        buttonSE = FindAnyObjectByType<ButtonSE>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        uiManager.currentState = UIState.TitleDefault;
        buttonSE.PlayButtonSE("Cancel");
    }
}
