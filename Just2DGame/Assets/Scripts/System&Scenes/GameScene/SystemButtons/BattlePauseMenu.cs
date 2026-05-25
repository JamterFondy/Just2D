using UnityEngine;
using TMPro;

public class BattlePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] TMP_Text stageName;
    UIManager uiManager;
    BattleESC battleESC;
    BattleFinish battleFinish;

    StageLoader stageLoader;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.StateChanged += OnStateChanged;
            UpdateVisibility(uiManager.currentState);
        }
        else
        {
            Debug.LogWarning("UIManager not found. Visibility won't update automatically.");
        }
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.BattlePauseMenu);
        
        if(stageLoader == null)
        {
            stageLoader = FindAnyObjectByType<StageLoader>();
        }

        stageName.text = stageLoader.stageName;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(battleESC == null) battleESC = FindAnyObjectByType<BattleESC>();
            battleESC.RestartBattle();
        }
    }

    public void OpenBattleSettings()
    {
        if(uiManager == null) uiManager = FindAnyObjectByType<UIManager>();

        uiManager.currentState = UIState.Settings;
    }

    public void QuitStageSelected()
    {
        if(uiManager == null) uiManager = FindAnyObjectByType<UIManager>();
        uiManager.currentState = UIState.BattleQuitConfirm;
    }

    public void CloseMenu()
    {
        if(battleESC == null) battleESC = FindAnyObjectByType<BattleESC>();
        battleESC.RestartBattle();
    }
}
