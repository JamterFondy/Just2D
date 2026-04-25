using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoCharacterSelect : MonoBehaviour
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
        uiManager.currentScene = SceneType.CharacterSelect;
        uiManager.currentState = UIState.CharaSelectDefault;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "CharacterSelectScene"));
    }
}
