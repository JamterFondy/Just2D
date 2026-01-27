using System;
using UnityEngine;

public enum UIState
{
    Home,
    Map,
    PauseMenu,
}

public class UIManager : MonoBehaviour
{
    public event Action<UIState> StateChanged;

    UIState _currentState;
    public UIState currentState
    {
        get => _currentState;
        set
        {
            if (_currentState == value) return;
            _currentState = value;
            StateChanged?.Invoke(_currentState);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        switch(currentState)
        {
            case UIState.Home:
                // Handle Home UI
                
                currentState = UIState.Home; 
                break;

            case UIState.Map:
                // Handle Map UI

                currentState = UIState.Map;
                break;

            case UIState.PauseMenu:
                // Handle Pause Menu UI

                currentState = UIState.PauseMenu;
                break;
        }

    }
}
