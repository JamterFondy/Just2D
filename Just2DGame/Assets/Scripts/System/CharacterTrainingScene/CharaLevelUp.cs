using UnityEngine;
using UnityEngine.UI;

public class CharaLevelUp : MonoBehaviour
{
    [SerializeField] CharaTrainingUIManager charaTrainingUIManager;

    void Awake()
    {
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();

        if (charaTrainingUIManager != null)
        {
            Debug.LogWarning("CharaTrainingUIManager not found. Visibility won't update automatically.");
        }
        
    }
    void Start()
    {
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnClick()
    {
        charaTrainingUIManager.currentState = CharaTrainingUIState.CharaChoose;
    }
}
