using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FromData : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    UIManager uiManager;
    SEManager seManager;


    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        seManager = FindAnyObjectByType<SEManager>();
    }


    public void OnClick()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        seManager.PlaySE("Button", "Confirm_Button", "Confirm");

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
