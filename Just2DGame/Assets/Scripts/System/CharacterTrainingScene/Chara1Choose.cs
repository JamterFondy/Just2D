using UnityEngine;
using UnityEngine.UI;
using System.IO;

[System.Serializable]

public class Chara1Choose : MonoBehaviour
{
    [SerializeField] CharaTrainingUIManager charaTrainingUIManager;
    [SerializeField] CharacterInfo characterInfo; // キャラクター情報を表示するUIコンポーネント

    public int charaID = 1; // キャラのID

    string GetCharacterJsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Character1.json");
    }

    void Awake()
    {
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();
        characterInfo = FindObjectOfType<CharacterInfo>();

        if (charaTrainingUIManager == null)
        {
            Debug.LogWarning("CharaTrainingUIManager not found. Visibility won't update automatically.");
        }

        if(characterInfo == null)
        {
            Debug.LogWarning("CharacterInfo not found. Character information won't be displayed.");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        
        characterInfo.LoadCharacter1FromJson();       

        charaTrainingUIManager.currentState = CharaTrainingUIState.CharaTraining;
    }
}
