using UnityEngine;
using UnityEngine.UI;

public class ResetLevel : MonoBehaviour
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
            if (characterInfo.id == "1")
            {
                if (int.Parse(characterInfo.level) > 1)
                {
                    characterInfo.hp = "100";
                    characterInfo.atk = "10";

                    characterInfo.level = "1";
                    Debug.Log("LevelUp: キャラクターレベルが1にリセットされました");


                    characterInfo.SetCharacter1Info(characterInfo.id, characterInfo.charaName, characterInfo.level, characterInfo.hp, characterInfo.atk); // キャラクターレベルアップ後の情報をCharacterInfoコンポーネントに反映

                    characterInfo.UpdateCharacter1Info();// キャラクターレベルアップ後の情報をJSONファイルに反映
                }
                else
                {
                    Debug.Log("LevelUp: キャラクターレベルは最小値に達しています");
                }
            }
        }
    }
}
