using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class CharaLevelUp : MonoBehaviour
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
        target.SetActive(state == UIState.CharaTrainingDefault);
    }


    public void OnClick()
    {
        UIManager.Instance.currentState = UIState.ChooseTrainChara;
    }
}
