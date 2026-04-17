using System.IO;
using UnityEngine;


public class CharacterInfo : MonoBehaviour
{
    [SerializeField] string charaName, level, hp, atk;

    string GetCharacter1JsonPath()
    {
        return Path.Combine(Application.persistentDataPath, "Character1.json");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }
}
