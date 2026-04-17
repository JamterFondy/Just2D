using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public enum CharaTrainingUIState
{
    None,
    CharaChoose,
    CharaTraining,
    Equipment,
}
public class CharaTrainingUIManager : MonoBehaviour
{
    [SerializeField] GameObject LevelUp,CharaChoose,CharaTraining,Equipment;


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

        if (currentState == CharaTrainingUIState.None)
        {
            LevelUp.SetActive(true);
            Equipment.SetActive(true);

            CharaChoose.SetActive(false);
            CharaTraining.SetActive(false);
        }
        else if (currentState == CharaTrainingUIState.CharaChoose)
        {
            CharaChoose.SetActive(true);

            LevelUp.SetActive(false);
            CharaTraining.SetActive(false);
            Equipment.SetActive(false);
        }
        else if (currentState == CharaTrainingUIState.CharaTraining)
        {
            CharaTraining.SetActive(true);

            LevelUp.SetActive(false);
            CharaChoose.SetActive(false);
            Equipment.SetActive(false);
        }
        else if (currentState == CharaTrainingUIState.Equipment)
        {
            Equipment.SetActive(false);
            LevelUp.SetActive(false);
            CharaChoose.SetActive(false);
            CharaTraining.SetActive(false);
        }
    }
}
