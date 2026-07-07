
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class StageSelectSwitch : MonoBehaviour
{
    [SerializeField] GameObject target; // 表示/非表示を切り替える対象（ボタン本体など）

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
        target.SetActive(state == UIState.StageInfo);
    }
}