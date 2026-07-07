using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

[System.Serializable]

public class Chara1Choose : MonoBehaviour
{
    [SerializeField] CharacterInfo characterInfo; // キャラクター情報を表示するUIコンポーネント
    [SerializeField] GameObject target;

    public int charaID = 1; // キャラのID

    string GetCharacterJsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Character1.json");
    }

    void Awake()
    {
        characterInfo = FindAnyObjectByType<CharacterInfo>();
        if(characterInfo == null)
        {
            Debug.LogWarning("CharacterInfo not found. Character information won't be displayed.");
        }


        if (target == null) target = this.gameObject;
        
        UIManager.Instance.StateChanged += OnStateChanged;
        UpdateVisibility(UIManager.Instance.currentState);
        
        
    }

    void OnDestroy()
    {
        UIManager.Instance.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.ChooseTrainChara);
    }

    public void OnClick()
    {     
        UIManager.Instance.currentState = UIState.CharaTraining;

        characterInfo.LoadCharacter1FromJson();
    }
}
