using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    [SerializeField] CharacterInfo characterInfo;

    int level, hp, atk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterInfo = FindObjectOfType<CharacterInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
       if (characterInfo != null)
        {
            if(characterInfo.id == "1")
            {
                if(int.Parse(characterInfo.level) < 99)
                {
                    characterInfo.level = (int.Parse(characterInfo.level) + 1).ToString();
                    Debug.Log("LevelUp: キャラクターレベルが " + characterInfo.level + " に上がりました");

                    characterInfo.hp = (int.Parse(characterInfo.hp) + int.Parse(characterInfo.level) * 5).ToString();
                    characterInfo.atk = (int.Parse(characterInfo.atk) + int.Parse(characterInfo.level) * 2).ToString();

                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.level, characterInfo.hp, characterInfo.atk); // キャラクターレベルアップ後の情報をCharacterInfoコンポーネントに反映

                    characterInfo.UpdateCharacter1Info();// キャラクターレベルアップ後の情報をJSONファイルに反映
                }
                else
                {
                    Debug.Log("LevelUp: キャラクターレベルは最大値に達しています");
                }
            }
            else if(characterInfo.id == "2")
            {
                if(int.Parse(characterInfo.level) < 99)
                {
                    characterInfo.level = (int.Parse(characterInfo.level) + 1).ToString();
                    Debug.Log("LevelUp: キャラクターレベルが " + characterInfo.level + " に上がりました");

                    characterInfo.hp = (int.Parse(characterInfo.hp) + int.Parse(characterInfo.level) * 5).ToString();
                    characterInfo.atk = (int.Parse(characterInfo.atk) + int.Parse(characterInfo.level) * 2).ToString();

                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.level, characterInfo.hp, characterInfo.atk); // キャラクターレベルアップ後の情報をCharacterInfoコンポーネントに反映

                    characterInfo.UpdateCharacter2Info();// キャラクターレベルアップ後の情報をJSONファイルに反映
                }
                else
                {
                    Debug.Log("LevelUp: キャラクターレベルは最大値に達しています");
                }
            }
        }
    }
}
