using UnityEngine;
using System;
using TMPro;

public class UpperTabsUIs : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] TextMeshProUGUI scrapNumText;

    public UIState preState;
    LoadingManager loadingManager;

    int scrapNum;

    void Awake()
    {
        if (target == null) target = this.gameObject;
       
        UIManager.Instance.SceneChanged += OnSceneChanged;
        UpdateVisibility(UIManager.Instance.currentScene);
       

        DontDestroyOnLoad(this.gameObject);
    }

    void OnEnable()
    {
        if(loadingManager == null) loadingManager = FindAnyObjectByType<LoadingManager>();


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
        UIManager.Instance.SceneChanged -= OnSceneChanged;
    }

    void OnSceneChanged(SceneType scene) => UpdateVisibility(scene);

    void UpdateVisibility(SceneType scene)
    {
        if (target == null) return;
        target.SetActive(scene != SceneType.Battle && scene != SceneType.Title && scene != SceneType.Loading);
    }

    public void GoBackHome()
    {
        if (UIManager.Instance.currentScene == SceneType.Home)
        {
            return; // すでにホームにいる場合は何もしない
        }
        else
        {
            loadingManager = FindAnyObjectByType<LoadingManager>();

            UIManager.Instance.currentScene = SceneType.Loading;
            UIManager.Instance.currentState = UIState.Loading;

            SEManager.Instance.PlaySE("Button", "GoHome_Button", "GoHome");

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }
    }

    public void OpenSettings()
    {
        if (UIManager.Instance.currentState != UIState.Settings)
        {
            preState = UIManager.Instance.currentState;
            UIManager.Instance.currentState = UIState.Settings;
            SEManager.Instance.PlaySE("Button", "Confirm_Button", "Confirm");
        }
        else
        {
            UIManager.Instance.currentState = preState;
            SEManager.Instance.PlaySE("Button", "Cancel_Button", "Cancel");
        }
    }


    // スクラップ数の変動をボタンのテキストに反映
    public void ScrapNumChanged()
    {
        if(PlayerPrefs.HasKey("ScrapNum"))
        {
            scrapNum = PlayerPrefs.GetInt("ScrapNum");
            scrapNumText.text = scrapNum.ToString();
        }
    }


    public void OnClickScrapCore()
    {
        
    }


}
