using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FromData : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    UIManager uiManager;

    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
        uiManager = FindObjectOfType<UIManager>();
    }


    public void OnClick()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
