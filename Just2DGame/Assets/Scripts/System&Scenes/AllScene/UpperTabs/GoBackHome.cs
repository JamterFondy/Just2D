using UnityEngine;

public class GoBackHome : MonoBehaviour
{
    LoadingManager loadingManager;
    UIManager uiManager;
    ButtonSE buttonSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
        uiManager = FindObjectOfType<UIManager>(); 
        buttonSE = FindObjectOfType<ButtonSE>();
    }

    public void OnClick()
    {
        if(uiManager.currentScene == SceneType.Home)
        {
            return; // Ç∑Ç≈Ç…ÉzÅ[ÉÄÇ…Ç¢ÇÈèÍçáÇÕâΩÇ‡ÇµÇ»Ç¢
        }
        else
        {
            loadingManager = FindObjectOfType<LoadingManager>();

            uiManager.currentScene = SceneType.Loading;
            uiManager.currentState = UIState.Loading;

            buttonSE.PlayButtonSE("GoHome");

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }

        
    }
}
