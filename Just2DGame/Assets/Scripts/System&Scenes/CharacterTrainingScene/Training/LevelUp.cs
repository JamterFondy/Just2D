using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour
{
    [SerializeField] CharacterInfo characterInfo;
    [SerializeField] UpperTabsUIs upperTabsUIs;

    int level, hp, atk;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterInfo = FindAnyObjectByType<CharacterInfo>();
        upperTabsUIs = FindAnyObjectByType<UpperTabsUIs>();
    }

    int CalculateRequiredScrap(int charaID, int currentLevel)
    {
        if (currentLevel == 1)
        {
            PlayerPrefs.SetInt($"LVUp_LastLevel_Chara{charaID}", currentLevel);

            PlayerPrefs.SetInt($"NecessaryEXP_Chara{charaID}", 100);
            return 100;          
        }
        else if(currentLevel == PlayerPrefs.GetInt($"LVUp_LastLevel_Chara{charaID}"))
        {
            PlayerPrefs.SetInt($"LVUp_LastLevel_Chara{charaID}", currentLevel);
            Debug.Log("ここが呼ばれている" + currentLevel);

            return PlayerPrefs.GetInt($"NecessaryEXP_Chara{charaID}");
        }
        else
        {
            PlayerPrefs.SetInt($"LVUp_LastLevel_Chara{charaID}", currentLevel);

            int pastNecessaryEXP = PlayerPrefs.GetInt($"NecessaryEXP_Chara{charaID}");
            int newNecessaryEXP = pastNecessaryEXP + 25 * currentLevel;
            PlayerPrefs.SetInt($"NecessaryEXP_Chara{charaID}", newNecessaryEXP);

            return newNecessaryEXP;
        }
    }


    public void OnClick()
    {
        int currentResources = PlayerPrefs.GetInt("ScrapNum"); // Y(n - 1) + 25 * nでレベルアップに必要なスクラップ数になる。


       if (characterInfo != null)
        {
            if(characterInfo.id == "1" && int.Parse(characterInfo.level) < 99) // キャラ１の場合の処理
            {
                int requiredScrap = CalculateRequiredScrap(int.Parse(characterInfo.id), int.Parse(characterInfo.level));
                Debug.Log("LevelUp: レベルアップに必要なスクラップ数は " + requiredScrap + " です");
                Debug.Log("LevelUp: 現在のスクラップ数は " + currentResources + " です");

                if (currentResources >= requiredScrap - int.Parse(characterInfo.exp))
                {
                    // スクラップを消費してレベルアップ
                    currentResources -= (requiredScrap - int.Parse(characterInfo.exp));
                    PlayerPrefs.SetInt("ScrapNum", currentResources);
                    Debug.Log("残りのスクラップ数は " + currentResources + " です");


                    // EXPとレベルの処理
                    characterInfo.exp = "0"; // レベルアップに必要なスクラップ数を満たしたのでEXPをリセット
                    characterInfo.level = (int.Parse(characterInfo.level) + 1).ToString();
                    Debug.Log("LevelUp: キャラクターレベルが " + characterInfo.level + " に上がりました");


                    // レベルアップによるステータスアップ
                    characterInfo.hp = (int.Parse(characterInfo.hp) + int.Parse(characterInfo.level) * 5).ToString();
                    characterInfo.atk = (int.Parse(characterInfo.atk) + int.Parse(characterInfo.level) * 2).ToString();


                    // レベルアップをJSONに反映
                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.exp, characterInfo.level, characterInfo.hp, characterInfo.atk, characterInfo.def, characterInfo.normalAttackLevel, characterInfo.skillLevel, characterInfo.ultLevel); // キャラクターレベルアップ後の情報をCharacterInfoコンポーネントに反映
                    characterInfo.UpdateCharacter1Info();// キャラクターレベルアップ後の情報をJSONファイルに反映


                }
                else if(currentResources < requiredScrap - int.Parse(characterInfo.exp))
                {
                    // スクラップを全て消費して現在EXP量をその分だけ上昇させる。
                    characterInfo.exp = (int.Parse(characterInfo.exp) + currentResources).ToString();
                    currentResources = 0;
                    PlayerPrefs.SetInt("ScrapNum", currentResources);
                    Debug.Log("残りのスクラップ数は " + currentResources + " です");


                    // EXPの増加をJSONに反映
                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.exp, characterInfo.level, characterInfo.hp, characterInfo.atk, characterInfo.def, characterInfo.normalAttackLevel, characterInfo.skillLevel, characterInfo.ultLevel); 
                    characterInfo.UpdateCharacter1Info();


                }

                // スクラップ数の変動をUpperTabsのスクラップ数表示に反映
                upperTabsUIs.ScrapNumChanged();

            }
            else if(characterInfo.id == "2" && int.Parse(characterInfo.level) < 99) // キャラ２に以降も同様の処理を行う。
            {
                int requiredScrap = CalculateRequiredScrap(int.Parse(characterInfo.id), int.Parse(characterInfo.level));

                if (currentResources >= requiredScrap - int.Parse(characterInfo.exp))
                {
                    currentResources -= (requiredScrap - int.Parse(characterInfo.exp));
                    PlayerPrefs.SetInt("ScrapNum", currentResources);


                    characterInfo.exp = 0.ToString();
                    characterInfo.level = (int.Parse(characterInfo.level) + 1).ToString();
                    Debug.Log("LevelUp: キャラクターレベルが " + characterInfo.level + " に上がりました");


                    characterInfo.hp = (int.Parse(characterInfo.hp) + int.Parse(characterInfo.level) * 5).ToString();
                    characterInfo.atk = (int.Parse(characterInfo.atk) + int.Parse(characterInfo.level) * 2).ToString();


                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.exp, characterInfo.level, characterInfo.hp, characterInfo.atk, characterInfo.def, characterInfo.normalAttackLevel, characterInfo.skillLevel, characterInfo.ultLevel); 
                    characterInfo.UpdateCharacter2Info();


                }
                else if(currentResources < requiredScrap - int.Parse(characterInfo.exp))
                {
                    characterInfo.exp = (int.Parse(characterInfo.exp) + currentResources).ToString();
                    currentResources = 0;
                    PlayerPrefs.SetInt("ScrapNum", currentResources);


                    characterInfo.SetCharacterInfo(characterInfo.id, characterInfo.charaName, characterInfo.exp, characterInfo.level, characterInfo.hp, characterInfo.atk, characterInfo.def, characterInfo.normalAttackLevel, characterInfo.skillLevel, characterInfo.ultLevel); 
                    characterInfo.UpdateCharacter2Info();


                }

                // スクラップ数の変動をUpperTabsのスクラップ数表示に反映
                upperTabsUIs.ScrapNumChanged();

            }
        }
    }
}
