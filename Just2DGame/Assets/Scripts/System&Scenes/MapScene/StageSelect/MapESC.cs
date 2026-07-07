using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
using static UnityEngine.GraphicsBuffer;

public class MapESC : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }


    public void OnClick()
    {
       if(UIManager.Instance.currentState == UIState.StageInfo)
       {
           UIManager.Instance.currentState = UIState.StageMapDefault;
       }
       else
       {
            if(UIManager.Instance.currentState == UIState.Settings) return; 

            UIManager.Instance.currentScene = SceneType.Loading;
            UIManager.Instance.currentState = UIState.Loading;


            LoadingManager.Instance.StartCoroutine(LoadingManager.Instance.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
       }
    }
}
