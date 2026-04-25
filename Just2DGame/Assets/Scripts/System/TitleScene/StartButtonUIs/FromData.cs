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
        uiManager.currentState = UIState.HomeDefault;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
