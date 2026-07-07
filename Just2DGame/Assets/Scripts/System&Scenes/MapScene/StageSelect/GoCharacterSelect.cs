using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoCharacterSelect : MonoBehaviour
{
    StageSelectManager stageSelectManager;

    void Start()
    {
        stageSelectManager = FindAnyObjectByType<StageSelectManager>();
    }


    public void OnClick()
    {
        UIManager.Instance.currentScene = SceneType.Loading;
        UIManager.Instance.currentState = UIState.Loading;

        stageSelectManager.DetermineStageLayout();

        LoadingManager.Instance.StartCoroutine(LoadingManager.Instance.LoadSceneWithLoadingScreen("LoadingScene", "CharacterSelectScene"));
    }
}
