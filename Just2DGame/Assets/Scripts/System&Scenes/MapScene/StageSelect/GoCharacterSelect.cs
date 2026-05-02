using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoCharacterSelect : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    UIManager uiManager;
    StageSelectManager stageSelectManager;

    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        uiManager = FindAnyObjectByType<UIManager>();
        stageSelectManager = FindAnyObjectByType<StageSelectManager>();
    }


    public void OnClick()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        stageSelectManager.DetermineStageLayout();

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "CharacterSelectScene"));
    }
}
