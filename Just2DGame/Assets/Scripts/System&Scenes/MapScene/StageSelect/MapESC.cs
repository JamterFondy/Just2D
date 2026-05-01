using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
using static UnityEngine.GraphicsBuffer;

public class MapESC : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;
    UIManager uiManager;



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
       if(uiManager.currentState == UIState.StageInfo)
       {
           uiManager.currentState = UIState.StageMapDefault;
       }
       else
       {
            if(uiManager.currentState == UIState.Settings) return; 

            uiManager.currentScene = SceneType.Loading;
            uiManager.currentState = UIState.Loading;


            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
       }
    }
}
