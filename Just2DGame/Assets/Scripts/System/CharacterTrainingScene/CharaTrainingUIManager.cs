using System;
using UnityEngine;


public enum CharaTrainingUIState
{
    None,
    CharaChoose,
    CharaTraining,
    Equipment,
}
public class CharaTrainingUIManager : MonoBehaviour
{
    public event Action<CharaTrainingUIState> StateChanged;

    CharaTrainingUIState _currentState;
    public CharaTrainingUIState currentState
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
        switch (currentState)
        {
            case CharaTrainingUIState.None:
                // Handle None UI
                currentState = CharaTrainingUIState.None;
                break;
            case CharaTrainingUIState.CharaChoose:
                // Handle Chara Choose UI
                currentState = CharaTrainingUIState.CharaChoose;
                break;

            case CharaTrainingUIState.CharaTraining:
                // Handle Chara Training UI
                currentState = CharaTrainingUIState.CharaTraining;
                break;

            case CharaTrainingUIState.Equipment:
                // Handle Equipment UI
                currentState = CharaTrainingUIState.Equipment;
                break;
        }
    }
}
