using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
using System.IO;

[System.Serializable]
public class CharacterData
{
    public int ID;
    public string Name;
    public int Level;
    public int HP;
    public int ATK;
}

public class Chara1Choose : MonoBehaviour
{
    [SerializeField] GameObject target; // 表示/非表示を切り替える対象（ボタン本体など）
    [SerializeField] CharaTrainingUIManager charaTrainingUIManager;
    [SerializeField] CharacterInfo characterInfo; // キャラクター情報を表示するUIコンポーネント

    public int charaID = 1; // キャラのID

    string GetCharacterJsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Character1.json");
    }

    void Awake()
    {
        if (target == null) target = this.gameObject;
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();
        characterInfo = FindObjectOfType<CharacterInfo>();

        if (charaTrainingUIManager != null)
        {
            charaTrainingUIManager.StateChanged += OnStateChanged;
            UpdateVisibility(charaTrainingUIManager.currentState);
        }
        else
        {
            Debug.LogWarning("CharaTrainingUIManager not found. Visibility won't update automatically.");
        }

        if(characterInfo == null)
        {
            Debug.LogWarning("CharacterInfo not found. Character information won't be displayed.");
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

    void OnDestroy()
    {
        if (charaTrainingUIManager != null) charaTrainingUIManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(CharaTrainingUIState state) => UpdateVisibility(state);

    void UpdateVisibility(CharaTrainingUIState state)
    {
        if (target == null) return;
        target.SetActive(state == CharaTrainingUIState.CharaChoose);
    }

    public void OnClick()
    {
        string path = GetCharacterJsonPath();

        if (!File.Exists(path))
        {
            // ファイルがなければ新規作成(初期値込みでキャラクターごとに作る)
            CharacterData newData = new CharacterData
            {
                ID = charaID,
                Name = "Owner",
                Level = 1,
                HP = 100,
                ATK = 10
            };
            string json = JsonUtility.ToJson(newData, true);
            File.WriteAllText(path, json);
            Debug.Log("新規Character1.jsonを作成しました: " + path);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);
            Debug.Log($"ID: {data.ID}, Name: {data.Name}, Level: {data.Level}, HP: {data.HP}, ATK: {data.ATK}");

        }
        else
        {
            // ファイルがあれば読み込み
            string json = File.ReadAllText(path);
            CharacterData data = JsonUtility.FromJson<CharacterData>(json);
            Debug.Log($"ID: {data.ID}, Name: {data.Name}, Level: {data.Level}, HP: {data.HP}, ATK: {data.ATK}");
        }

        if (characterInfo != null)
        {
            
        }


        charaTrainingUIManager.currentState = CharaTrainingUIState.CharaTraining;
    }
}
