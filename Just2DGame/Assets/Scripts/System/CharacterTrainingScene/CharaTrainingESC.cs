using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class CharaTrainingESC : MonoBehaviour
{
    [SerializeField] CharaTrainingUIManager charaTrainingUIManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();
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
        if (charaTrainingUIManager.currentState == CharaTrainingUIState.CharaTraining)
        {
            charaTrainingUIManager.currentState = CharaTrainingUIState.CharaChoose;
        }
        else if(charaTrainingUIManager.currentState == CharaTrainingUIState.CharaChoose)
        {
            charaTrainingUIManager.currentState = CharaTrainingUIState.None;
        }
        else if(charaTrainingUIManager.currentState == CharaTrainingUIState.Equipment)
        {
            charaTrainingUIManager.currentState = CharaTrainingUIState.None;
        }
        else
        {
            SceneManager.LoadScene("HomeScene");
        }
        
    }
}
