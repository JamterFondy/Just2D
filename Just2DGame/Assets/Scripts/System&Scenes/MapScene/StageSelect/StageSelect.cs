using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
{
    UIManager uiManager;
    StageSelectManager stageSelectManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        stageSelectManager = FindObjectOfType<StageSelectManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        uiManager.currentState = UIState.StageInfo;
        stageSelectManager.stageNum = 11;
    }
}
