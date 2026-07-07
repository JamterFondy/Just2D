using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GoToBattleMap : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] LoadingManager loadingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (target == null) target = this.gameObject;
        
        UIManager.Instance.StateChanged += OnStateChanged;
        UpdateVisibility(UIManager.Instance.currentState);
    }

    void Start()
    {
        if (loadingManager == null)
        {
            loadingManager = FindAnyObjectByType<LoadingManager>();
            if (loadingManager == null)
            {
                Debug.LogError("LoadingManager not found. Please assign it in the inspector.");
            }
        }
    }

    void OnDestroy()
    {
        UIManager.Instance.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.HomeDefault);
    }

    public void OnClick()
    {
        UIManager.Instance.currentScene = SceneType.Loading;
        UIManager.Instance.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));
    } 
}
