using UnityEngine;
using UnityEngine.UI;

public class ResetLevel : MonoBehaviour
{
    [SerializeField] CharacterInfo characterInfo;

    int level, hp, atk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterInfo = FindAnyObjectByType<CharacterInfo>();
    }

    public void OnClick()
    {
        if (characterInfo != null)
        {
            if (characterInfo.id == "1")
            {
                if (int.Parse(characterInfo.level) > 1)
                {
                    characterInfo.hp = "100";
                    characterInfo.atk = "10";
                    characterInfo.def = "5";
                    characterInfo.exp = "0";
                    characterInfo.level = "1";
                    characterInfo.normalAttackLevel = "1";
                    characterInfo.skillLevel = "1";
                    characterInfo.ultLevel = "1";

                    Debug.Log("LevelUp: キャラクターレベルが1にリセットされました");


                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.exp, characterInfo.level, characterInfo.hp, characterInfo.atk, characterInfo.def, characterInfo.normalAttackLevel, characterInfo.skillLevel, characterInfo.ultLevel); // キャラクターレベルアップ後の情報をCharacterInfoコンポーネントに反映

                    characterInfo.UpdateCharacter1Info();// キャラクターレベルリセット後の情報をJSONファイルに反映
                }
                else
                {
                    Debug.Log("LevelUp: キャラクターレベルは最小値に達しています");
                }
            }
            else if (characterInfo.id == "2")
            {
                if (int.Parse(characterInfo.level) > 1)
                {
                    characterInfo.hp = "110";
                    characterInfo.atk = "8";
                    characterInfo.def = "3";
                    characterInfo.exp = "0";
                    characterInfo.level = "1";
                    characterInfo.normalAttackLevel = "1";
                    characterInfo.skillLevel = "1";
                    characterInfo.ultLevel = "1";

                    Debug.Log("LevelUp: キャラクターレベルが1にリセットされました");

                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.exp, characterInfo.level, characterInfo.hp, characterInfo.atk, characterInfo.def, characterInfo.normalAttackLevel, characterInfo.skillLevel, characterInfo.ultLevel); // キャラクターレベルアップ後の情報をCharacterInfoコンポーネントに反映

                    characterInfo.UpdateCharacter2Info();
                }
                else
                {
                    Debug.Log("LevelUp: キャラクターレベルは最小値に達しています");
                }
            }
        }
    }
}
