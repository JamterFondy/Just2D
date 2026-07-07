using UnityEngine;
using TMPro;

public class BattlePauseMenu : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] TMP_Text stageName;
    BattleESC battleESC;
    BattleFinish battleFinish;

    StageLoader stageLoader;

    void Awake()
    {
        if (target == null) target = this.gameObject;
       
        UIManager.Instance.StateChanged += OnStateChanged;
        UpdateVisibility(UIManager.Instance.currentState);
       
    }

    void OnDestroy()
    {
        UIManager.Instance.StateChanged -= OnStateChanged;
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
        UIManager.Instance.currentState = UIState.Settings;
    }

    public void QuitStageSelected()
    {
        UIManager.Instance.currentState = UIState.BattleQuitConfirm;
    }

    public void CloseMenu()
    {
        if(battleESC == null) battleESC = FindAnyObjectByType<BattleESC>();
        battleESC.RestartBattle();
    }
}
