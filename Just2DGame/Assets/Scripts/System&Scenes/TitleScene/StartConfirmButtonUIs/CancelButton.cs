using UnityEngine;
using System;

public class CancelButton : MonoBehaviour
{
    UIManager uiManager;
    SEManager seManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        seManager = FindAnyObjectByType<SEManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        uiManager.currentState = UIState.TitleDefault;
        seManager.PlaySE("Button", "Cancel_Button", "Cancel");
    }
}
