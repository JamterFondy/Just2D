using UnityEngine;
using System;

public enum TitleUIState
{
    Default,//デフォルト
    Settings,//設定画面
    StartConfirm,//ゲーム開始確認
    Credit,//クレジット
}

public class TitleUIManager : MonoBehaviour
{
    [SerializeField] GameObject StartButton, SettingsButton,SettingUI, StartConfirmUI, CreditUI;

    public event Action<TitleUIState> StateChanged;

    TitleUIState _currentState;

    public TitleUIState currentState
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
            case TitleUIState.Default:
                // Handle Default UI
                currentState = TitleUIState.Default;

                StartButton.SetActive(true);
                SettingsButton.SetActive(true);
                SettingUI.SetActive(false);
                StartConfirmUI.SetActive(false);
                CreditUI.SetActive(false);

                break;

             case TitleUIState.Settings:
                // Handle Settings UI
                currentState = TitleUIState.Settings;

                SettingUI.SetActive(true);

                StartButton.SetActive(false);
                SettingsButton.SetActive(false);
                StartConfirmUI.SetActive(false);
                CreditUI.SetActive(false);

                break;

             case TitleUIState.StartConfirm:
                // Handle Start Confirm UI
                currentState = TitleUIState.StartConfirm;

                StartConfirmUI.SetActive(true);

                StartButton.SetActive(false);
                SettingsButton.SetActive(false);
                SettingUI.SetActive(false);
                CreditUI.SetActive(false);

                break;

             case TitleUIState.Credit:
                // Handle Credit UI
                currentState = TitleUIState.Credit;

                CreditUI.SetActive(true);

                StartButton.SetActive(false);
                SettingsButton.SetActive(false);
                SettingUI.SetActive(false);
                StartConfirmUI.SetActive(false);

                break;
        }

    }
    
}
