using UnityEngine;
using System;

public class CancelButton : MonoBehaviour
{
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        uiManager.currentState = UIState.TitleDefault;
    }
}
