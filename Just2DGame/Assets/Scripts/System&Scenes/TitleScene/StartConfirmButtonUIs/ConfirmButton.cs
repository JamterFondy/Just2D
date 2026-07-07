using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class ConfirmButton : MonoBehaviour
{
    public void OnClick()
    {
        // 1) 指定したフォルダ内のJSONファイルを削除
        try
        {
            string folder = Application.persistentDataPath;
            if (Directory.Exists(folder))
            {
                var files = Directory.GetFiles(folder, "*.json", SearchOption.TopDirectoryOnly);
                foreach (var f in files)
                {
                    try { File.Delete(f); }
                    catch (System.Exception ex) { Debug.LogWarning($"ConfirmButton: Failed to delete json '{f}': {ex}"); }
                }
            }
            else
            {
                Debug.LogWarning($"ConfirmButton: persistentDataPath does not exist: {folder}");
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning($"ConfirmButton: Error deleting JSON files: {ex}");
        }

        // 2) 設定関連以外のPlayerPrefsを削除
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float se = PlayerPrefs.GetFloat("SEVolume", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("MasterVolume", master);
        PlayerPrefs.SetFloat("BGMVolume", bgm);
        PlayerPrefs.SetFloat("SEVolume", se);
        PlayerPrefs.SetInt("GameProgress", 0); //チュートリアルからの開始になる。続きからを選択できなくなる。
        PlayerPrefs.Save();


        //3)Character1.jsonの作成 & 初期化⇒チュートリアルの戦闘では事前にキャラ育成画面に行かないためここで作っておく

        string path = Path.Combine(Application.persistentDataPath, "Character1.json");

        CharacterData data;
        if (!File.Exists(path))
        {
            data = new CharacterData
            {
                ID = 1,
                Name = "Owner",
                EXP = 0,
                Level = 1,
                HP = 100,
                ATK = 10,
                DEF = 5,
                NormalAttackLevel = 1,
                SkillLevel = 1,
                UltLevel = 1
            };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log("CharacterInfo: 新規Character1.jsonを作成しました: " + path);
        }


        // 4) ローディングの開始処理
        UIManager.Instance.currentScene = SceneType.Loading;
        UIManager.Instance.currentState = UIState.Loading;

        SEManager.Instance.PlaySE("Button", "Confirm_Button", "Confirm");

        LoadingManager.Instance.StartCoroutine(LoadingManager.Instance.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        
    }
}

