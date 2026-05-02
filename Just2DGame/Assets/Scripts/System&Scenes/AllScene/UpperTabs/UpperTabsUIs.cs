using UnityEngine;
using System;

public class UpperTabsUIs : MonoBehaviour
{
    [SerializeField] GameObject target;
    UIManager uiManager;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.SceneChanged += OnSceneChanged;
            UpdateVisibility(uiManager.currentScene);
        }
        else
        {
            Debug.LogWarning("UIManager not found. Visibility won't update automatically.");
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.SceneChanged -= OnSceneChanged;
    }

    void OnSceneChanged(SceneType scene) => UpdateVisibility(scene);

    void UpdateVisibility(SceneType scene)
    {
        if (target == null) return;
        target.SetActive(scene != SceneType.Battle && scene != SceneType.Title);
    }
}
