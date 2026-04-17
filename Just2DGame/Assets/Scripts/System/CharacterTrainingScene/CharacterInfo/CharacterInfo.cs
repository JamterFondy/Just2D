using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterData
{
    public int ID;
    public string Name;
    public int Level;
    public int HP;
    public int ATK;
}
public class CharacterInfo : MonoBehaviour
{
    [SerializeField] string charaName, level, hp, atk;

    // Cached references to child text UI components (supports Unity UI Text and TextMeshProUGUI)
    Text nameText;
    Text levelText;
    Text hpText;
    Text atkText;

    TMPro.TextMeshProUGUI tmpNameText;
    TMPro.TextMeshProUGUI tmpLevelText;
    TMPro.TextMeshProUGUI tmpHpText;
    TMPro.TextMeshProUGUI tmpAtkText;

    string GetCharacter1JsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Character1.json");
    }

    // Load Character1 data from JSON file. If file does not exist, create default data.
    public void LoadCharacter1FromJson()
    {
        string path = GetCharacter1JsonPath();

        CharacterData data;
        if (!File.Exists(path))
        {
            data = new CharacterData
            {
                ID = 1,
                Name = "Owner",
                Level = 1,
                HP = 100,
                ATK = 10
            };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log("CharacterInfo: 新規Character1.jsonを作成しました: " + path);
        }
        else
        {
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<CharacterData>(json);
        }

        // Apply loaded data to this component
        SetCharacter1Info(data.Name, data.Level.ToString(), data.HP.ToString(), data.ATK.ToString());
    }

    // Allow setting info from a CharacterData instance
    public void SetCharacterInfoFromData(CharacterData data)
    {
        if (data == null) return;
        SetCharacter1Info(data.Name, data.Level.ToString(), data.HP.ToString(), data.ATK.ToString());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // キャラクター情報 UI 配下のテキスト等がボタンのクリックを阻害しないようにする
        DisableChildGraphicsRaycast();
        CacheTextComponents();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacter1Info(string name, string level, string hp, string atk)
    {
        this.charaName = name;
        this.level = level;
        this.hp = hp;
        this.atk = atk;
        UpdateUIText();
    }

    void CacheTextComponents()
    {
        // Find UnityEngine.UI.Text components by child GameObject name
        var texts = GetComponentsInChildren<Text>(true);
        foreach (var t in texts)
        {
            var n = t.gameObject.name.ToLower();
            if (n.Contains("name")) nameText = t;
            else if (n.Contains("level")) levelText = t;
            else if (n.Contains("hp")) hpText = t;
            else if (n.Contains("atk")) atkText = t;
        }

        // Find TextMeshProUGUI components as well
        var tmps = GetComponentsInChildren<TMPro.TextMeshProUGUI>(true);
        foreach (var t in tmps)
        {
            var n = t.gameObject.name.ToLower();
            if (n.Contains("name")) tmpNameText = t;
            else if (n.Contains("level")) tmpLevelText = t;
            else if (n.Contains("hp")) tmpHpText = t;
            else if (n.Contains("atk")) tmpAtkText = t;
        }
    }

    void UpdateUIText()
    {
        // Ensure components are cached
        if (nameText == null && tmpNameText == null) CacheTextComponents();

        if (nameText != null) nameText.text = charaName;
        if (levelText != null) levelText.text = "Lv." + level;
        if (hpText != null) hpText.text = "HP: " + hp;
        if (atkText != null) atkText.text = "ATK: " + atk;

        if (tmpNameText != null) tmpNameText.text = charaName;
        if (tmpLevelText != null) tmpLevelText.text = "Lv. " + level;
        if (tmpHpText != null) tmpHpText.text = "HP: " + hp;
        if (tmpAtkText != null) tmpAtkText.text = "ATK: " + atk;
    }

    // このオブジェクト配下の UI Graphic（Image / Text / TMP 等）の raycastTarget を無効化します。
    // これにより、重なっている UI テキストが下層のボタン等のクリック判定を妨げなくなります。
    void DisableChildGraphicsRaycast()
    {
        var graphics = GetComponentsInChildren<Graphic>(true);
        foreach (var g in graphics)
        {
            g.raycastTarget = false;
        }
    }
}
