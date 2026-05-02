using UnityEngine;

public class GoBackHome : MonoBehaviour
{
    LoadingManager loadingManager;
    UIManager uiManager;
    ButtonSE buttonSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        uiManager = FindAnyObjectByType<UIManager>(); 
        buttonSE = FindAnyObjectByType<ButtonSE>();
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

            buttonSE.PlayButtonSE("GoHome");

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }

        
    }
}
