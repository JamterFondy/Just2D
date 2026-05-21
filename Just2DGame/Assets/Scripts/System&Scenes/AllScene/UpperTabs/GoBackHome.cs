using UnityEngine;

public class GoBackHome : MonoBehaviour
{
    LoadingManager loadingManager;
    UIManager uiManager;
    SEManager seManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        uiManager = FindAnyObjectByType<UIManager>(); 
        seManager = FindAnyObjectByType<SEManager>();
    }

    public void OnClick()
    {
        if(uiManager.currentScene == SceneType.Home)
        {
            return; // Ç∑Ç≈Ç…ÉzÅ[ÉÄÇ…Ç¢ÇÈèÍçáÇÕâΩÇ‡ÇµÇ»Ç¢
        }
        else
        {
            loadingManager = FindAnyObjectByType<LoadingManager>();

            uiManager.currentScene = SceneType.Loading;
            uiManager.currentState = UIState.Loading;

            seManager.PlaySE("Button", "GoHome_Button");

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }

        
    }
}
