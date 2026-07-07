using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FromData : MonoBehaviour
{
    public void OnClick()
    {
        UIManager.Instance.currentScene = SceneType.Loading;
        UIManager.Instance.currentState = UIState.Loading;

        SEManager.Instance.PlaySE("Button", "Confirm_Button", "Confirm");

        LoadingManager.Instance.StartCoroutine(LoadingManager.Instance.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));

    }
}
