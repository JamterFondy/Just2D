using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharaTraining : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "CharacterTrainingScene"));
    }
}
