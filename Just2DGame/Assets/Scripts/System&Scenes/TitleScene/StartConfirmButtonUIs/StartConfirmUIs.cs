using UnityEngine;
using System;

public class StartConfirmUIs : MonoBehaviour
{
    [SerializeField] GameObject target;
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
        target.SetActive(state == UIState.GameStartConfirm);
    }
}
