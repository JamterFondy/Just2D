using UnityEngine;
using System;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] GameObject target;

    void Awake()
    {
        if (target == null) target = this.gameObject;
               
        UIManager.Instance.StateChanged += OnStateChanged;
        UpdateVisibility(UIManager.Instance.currentState);
       
        DontDestroyOnLoad(this.gameObject);
    }


    void OnDestroy()
    {
        UIManager.Instance.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.Settings);
    }

}
