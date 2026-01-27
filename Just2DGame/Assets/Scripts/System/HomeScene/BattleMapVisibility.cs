
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class BattleMapVisibility : MonoBehaviour
{
    [SerializeField] GameObject target; // 表示/非表示を切り替える対象（ボタン本体など）
    UIManager uiManager;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindObjectOfType<UIManager>();
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
        target.SetActive(state == UIState.Map);
    }
}