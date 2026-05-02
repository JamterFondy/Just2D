using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FromData : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    UIManager uiManager;
    ButtonSE buttonSE;

    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        buttonSE = FindAnyObjectByType<ButtonSE>();
    }


    public void OnClick()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        buttonSE.PlayButtonSE("Confirm");

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
