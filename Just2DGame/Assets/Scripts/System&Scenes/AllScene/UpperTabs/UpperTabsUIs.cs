using UnityEngine;
using System;
using TMPro;

public class UpperTabsUIs : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] TextMeshProUGUI scrapNumText;

    UIManager uiManager;
    public UIState preState;
    LoadingManager loadingManager;
    SEManager seManager;

    int scrapNum;

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

    void OnEnable()
    {
        if(loadingManager == null) loadingManager = FindAnyObjectByType<LoadingManager>();
        if(seManager == null) seManager = FindAnyObjectByType<SEManager>();


        if (PlayerPrefs.HasKey("ScrapNum"))
        {
            scrapNum = PlayerPrefs.GetInt("ScrapNum");
            scrapNumText.text = scrapNum.ToString();
        }
        else
        {
            scrapNum = 0;
            scrapNumText.text = "0";
        }
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

    public void GoBackHome()
    {
        if (uiManager.currentScene == SceneType.Home)
        {
            return; // すでにホームにいる場合は何もしない
        }
        else
        {
            loadingManager = FindAnyObjectByType<LoadingManager>();

            uiManager.currentScene = SceneType.Loading;
            uiManager.currentState = UIState.Loading;

            seManager.PlaySE("Button", "GoHome_Button", "GoHome");

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }
    }


    public void OpenSettings()
    {
        if (uiManager.currentState != UIState.Settings)
        {
            preState = uiManager.currentState;
            uiManager.currentState = UIState.Settings;
            seManager.PlaySE("Button", "Confirm_Button", "Confirm");
        }
        else
        {
            uiManager.currentState = preState;
            seManager.PlaySE("Button", "Cancel_Button", "Cancel");
        }
    }


    public void OnClickScrapCore()
    {
        
    }


}
