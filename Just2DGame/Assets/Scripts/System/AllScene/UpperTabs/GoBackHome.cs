using UnityEngine;

public class GoBackHome : MonoBehaviour
{
    LoadingManager loadingManager;
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
        uiManager = FindObjectOfType<UIManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
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

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }

        
    }
}
