
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class StageSelectSwitch : MonoBehaviour
{
    [SerializeField] GameObject target; // 表示/非表示を切り替える対象（ボタン本体など）
    MapUIManager mapUIManager;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        mapUIManager = FindObjectOfType<MapUIManager>();
        if (mapUIManager != null)
        {
            mapUIManager.StateChanged += OnStateChanged;
            UpdateVisibility(mapUIManager.currentState);
        }
        else
        {
            Debug.LogWarning("MapUIManager not found. Visibility won't update automatically.");
        }
    }

    void OnDestroy()
    {
        if (mapUIManager != null) mapUIManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(MapUIState state) => UpdateVisibility(state);

    void UpdateVisibility(MapUIState state)
    {
        if (target == null) return;
        target.SetActive(state == MapUIState.MapSelect);
    }
}