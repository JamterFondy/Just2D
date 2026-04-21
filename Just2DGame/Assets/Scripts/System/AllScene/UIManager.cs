using System;
using UnityEngine;

public enum UIState
{
    //全シーン共通の状態
    Settings,

    //タイトル画面の状態
    TitleDefault,
    GameStartConfirm,
    Credit,

    //ホーム画面の状態
    HomeDefault,
    PauseMenu,

    //キャラ育成画面の状態
    CharaTrainingDefault,
    ChooseTrainChara,
    CharaTraining,
    ChooseEquipment,

    //ステージマップの状態
    StageMapDefault,
    StageInfo,
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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        currentState = UIState.TitleDefault; // 初期状態をタイトル画面のデフォルトに設定
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
