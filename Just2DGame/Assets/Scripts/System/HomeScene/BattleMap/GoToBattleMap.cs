using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GoToBattleMap : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] LoadingManager loadingManager;

    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    void Start()
    {
        if (loadingManager == null)
        {
            loadingManager = FindObjectOfType<LoadingManager>();
            if (loadingManager == null)
            {
                Debug.LogError("LoadingManager not found. Please assign it in the inspector.");
            }
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
        target.SetActive(state == UIState.HomeDefault);
    }

    public void OnClick()
    {
        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));
    } 
}
