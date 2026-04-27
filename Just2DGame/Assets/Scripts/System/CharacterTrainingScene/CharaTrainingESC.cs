using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class CharaTrainingESC : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        loadingManager = FindObjectOfType<LoadingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }


    public void OnClick()
    {
        if (uiManager.currentState == UIState.CharaTraining)
        {
            uiManager.currentState = UIState.ChooseTrainChara;
        }
        else if(uiManager.currentState == UIState.ChooseTrainChara)
        {
            uiManager.currentState = UIState.CharaTrainingDefault;
        }
        else if(uiManager.currentState == UIState.ChooseEquipment)
        {
            uiManager.currentState = UIState.CharaTrainingDefault;
        }
        else
        {
            if(uiManager.currentState == UIState.Settings) return;

            uiManager.currentScene = SceneType.Home;
            uiManager.currentState = UIState.HomeDefault;

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }
        
    }
}
