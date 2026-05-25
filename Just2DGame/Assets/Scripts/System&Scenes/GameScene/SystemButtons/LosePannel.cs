using TMPro;
using UnityEngine;

public class LosePannel : MonoBehaviour
{
    [SerializeField] GameObject target;
    UIManager uiManager;
    StageLoader stageLoader;
    BattleFinish battleFinish;
    BattleESC battleESC;

    [SerializeField] TextMeshProUGUI stageName;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        stageLoader = FindAnyObjectByType<StageLoader>();

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
        target.SetActive(state == UIState.PlayerLose);

        if (stageLoader == null)
        {
            stageLoader = FindAnyObjectByType<StageLoader>();
        }

        stageName.text = stageLoader.stageName;
    }

    public void RetryStage()
    {
        if (battleFinish == null) battleFinish = FindAnyObjectByType<BattleFinish>();

        battleFinish.TryAgain();
    }

    public void QuitStage()
    {
        if (battleESC == null) battleESC = FindAnyObjectByType<BattleESC>();
        if (battleFinish == null) battleFinish = FindAnyObjectByType<BattleFinish>();

        battleESC.QuitStage();
        battleFinish.QuitStage();
    }

}
