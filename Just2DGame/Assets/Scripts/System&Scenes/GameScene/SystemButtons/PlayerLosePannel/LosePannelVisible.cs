using TMPro;
using UnityEngine;

public class LosePannelVisible : MonoBehaviour
{
    [SerializeField] GameObject target;
    UIManager uiManager;
    StageLoader stageLoader;

    [SerializeField] TextMeshProUGUI stageName;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        stageLoader = FindAnyObjectByType<StageLoader>();

        stageName.text = stageLoader.stageName;

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

    }
}
