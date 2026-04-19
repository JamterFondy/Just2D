using System.IO;
using UnityEngine;

[System.Serializable]
public class CharacterDataSmall
{
    public int ID;
    public string Name;
    public int Level;
    public int HP;
    public int ATK;
}

public class CharaInfoServer : MonoBehaviour
{
    public int ID, NAME, HP, ATK;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterInfo()//GameSceneにいるPlayerStatusに渡すキャラクターの情報を受け取る関数
    {
        // ID に応じて対応する Character JSON を読み込み、NAME / HP / ATK に値を設定する
        // (ID 自身は変更しない)
        if (ID != 1 && ID != 2)
        {
            Debug.LogWarning($"CharaInfoServer: Unsupported ID {ID}. Expected 1 or 2.");
            return;
        }

        string fileName = $"Character{ID}.json";
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(path))
        {
            Debug.LogWarning($"CharaInfoServer: Character file not found: {path}");
            return;
        }

        try
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<CharacterDataSmall>(json);
            if (data != null)
            {
                // NAME はプロジェクト内で int として定義されているため JSON の ID を代入する
                NAME = data.ID;
                HP = data.HP;
                ATK = data.ATK;
                Debug.Log($"CharaInfoServer: Loaded Character{ID} -> NAME(ID):{NAME}, HP:{HP}, ATK:{ATK}");
            }
            else
            {
                Debug.LogWarning("CharaInfoServer: Failed to parse character JSON.");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"CharaInfoServer: Error reading character file {path}: {ex}");
        }
    }

}
