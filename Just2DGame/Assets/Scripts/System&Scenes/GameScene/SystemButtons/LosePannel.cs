using TMPro;
using UnityEngine;

public class LosePannel : MonoBehaviour
{
    [SerializeField] GameObject target;
    StageLoader stageLoader;
    BattleFinish battleFinish;
    BattleESC battleESC;

    [SerializeField] TextMeshProUGUI stageName;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        stageLoader = FindAnyObjectByType<StageLoader>();

        
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
