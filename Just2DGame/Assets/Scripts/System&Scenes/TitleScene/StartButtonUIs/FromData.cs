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
        loadingManager = FindObjectOfType<LoadingManager>();
        uiManager = FindObjectOfType<UIManager>();
        buttonSE = FindObjectOfType<ButtonSE>();
    }


    public void OnClick()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        buttonSE.PlayButtonSE("Confirm");

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
