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
    UIManager uiManager;

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
        uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.StateChanged += OnStateChanged;
            UpdateVisibility(uiManager.currentState);
        }
        else
        {
            Debug.LogWarning("UIManager not found. Visibility won't update automatically.");
        }
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.ChooseTrainChara);
    }

    public void OnClick()
    {     
        uiManager.currentState = UIState.CharaTraining;

        characterInfo.LoadCharacter1FromJson();
    }
}
