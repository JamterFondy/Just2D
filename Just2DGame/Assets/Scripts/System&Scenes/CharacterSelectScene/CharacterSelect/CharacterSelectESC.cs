using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectESC : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] GameObject charaInfoServer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        charaInfoServer = FindAnyObjectByType<CharaInfoServer>().gameObject;
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
        if (UIManager.Instance.currentState == UIState.Settings) return; 

        Destroy(charaInfoServer);

        UIManager.Instance.currentScene = SceneType.Loading;
        UIManager.Instance.currentState = UIState.Loading;


        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));
    }
}
