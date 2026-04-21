using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGameFromData : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] GameObject target;
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

    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.TitleDefault);
    }

    public void OnClick()
    {
        uiManager.currentState = UIState.HomeDefault;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
