using System;
using UnityEngine;

public enum MapUIState
{
    Map,
    MapSelect,
}

public class MapUIManager : MonoBehaviour
{
    public event Action<MapUIState> StateChanged;

    MapUIState _currentState;
    public MapUIState currentState
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
            case MapUIState.Map:
                // Handle Map UI

                currentState = MapUIState.Map;
                break;

            case MapUIState.MapSelect:
                // Handle Map Select UI

                currentState = MapUIState.MapSelect;
                break;
        }

    }
}
