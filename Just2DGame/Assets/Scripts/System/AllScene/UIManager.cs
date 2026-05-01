using System;
using UnityEngine;

public enum SceneType
{
    Loading,
    Title,
    Home,
    CharacterTraining,
    Map,
    CharacterSelect,
    Battle,
}

public enum UIState
{
    //全シーン共通の状態
    Settings,
    Loading,

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

    //キャラ選択画面の状態
    CharaSelectDefault,

    //バトル中の状態
    InBattle,
}

public class UIManager : MonoBehaviour
{
    public event Action<SceneType> SceneChanged;
    public event Action<UIState> StateChanged;

    SceneType _currentScene;
    UIState _currentState;

    public SceneType currentScene //シーンを跨ぐUI管理のためのプロパティ
    {
        get => _currentScene;
        set
        {
            if (_currentScene == value) return;
            _currentScene = value;
            SceneChanged?.Invoke(_currentScene);
        }
    }
    public UIState currentState //シーン内の状態で変化するUI管理のためのプロパティ
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

        currentScene = SceneType.Title; // 初期シーンをタイトルに設定
        currentState = UIState.TitleDefault; // 初期状態をタイトル画面のデフォルトに設定
    }

    public void LoadSceneRecive(string sceneName)
    {
        if(sceneName == "TitleScene")
        {
            currentScene = SceneType.Title;
            currentState = UIState.TitleDefault;
        }
        else if (sceneName == "HomeScene")
        {
            currentScene = SceneType.Home;
            currentState = UIState.HomeDefault;

        }
        else if (sceneName == "CharacterTrainingScene")
        {
            currentScene = SceneType.CharacterTraining;
            currentState = UIState.CharaTrainingDefault;
        }
        else if (sceneName == "MapScene")
        {
            currentScene = SceneType.Map;
            currentState = UIState.StageMapDefault;
        }
        else if (sceneName == "CharacterSelectScene")
        {
            currentScene = SceneType.CharacterSelect;
            currentState = UIState.CharaSelectDefault;
        }
        else if (sceneName == "GameScene")
        {
            currentScene = SceneType.Battle;
            currentState = UIState.InBattle;
        }
    }
}
