using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectESC : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] GameObject charaInfoServer;
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
        charaInfoServer = FindObjectOfType<CharaInfoServer>().gameObject;
        uiManager = FindObjectOfType<UIManager>();
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
        if (uiManager.currentState == UIState.Settings) return; 

        Destroy(charaInfoServer);

        uiManager.currentScene = SceneType.Map;
        uiManager.currentState = UIState.StageInfo;


        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));
    }
}
