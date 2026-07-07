using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class CharaTrainingESC : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
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
        if (UIManager.Instance.currentState == UIState.CharaTraining)
        {
            UIManager.Instance.currentState = UIState.ChooseTrainChara;
        }
        else if(UIManager.Instance.currentState == UIState.ChooseTrainChara)
        {
            UIManager.Instance.currentState = UIState.CharaTrainingDefault;
        }
        else if(UIManager.Instance.currentState == UIState.ChooseEquipment)
        {
            UIManager.Instance.currentState = UIState.CharaTrainingDefault;
        }
        else
        {
            if(UIManager.Instance.currentState == UIState.Settings) return;

            UIManager.Instance.currentScene = SceneType.Loading;
            UIManager.Instance.currentState = UIState.Loading;

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }
        
    }
}
