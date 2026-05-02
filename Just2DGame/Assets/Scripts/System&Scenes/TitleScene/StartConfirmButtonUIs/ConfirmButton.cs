using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class ConfirmButton : MonoBehaviour
{
    UIManager uiManager;
    ButtonSE buttonSE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        buttonSE = FindAnyObjectByType<ButtonSE>();
    }

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
                Level = 1,
                HP = 100,
                ATK = 10
            };
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log("CharacterInfo: 新規Character1.jsonを作成しました: " + path);
        }


        // 4) Start loading via LoadingManager
        var loadingManager = FindAnyObjectByType<LoadingManager>();
        if (loadingManager == null)
        {
            // Create a LoadingManager if none exists so coroutine can run
            var go = new GameObject("LoadingManager");
            loadingManager = go.AddComponent<LoadingManager>();
        }

        if (loadingManager != null)
        {
            uiManager.currentScene = SceneType.Loading;
            uiManager.currentState = UIState.Loading;

            buttonSE.PlayButtonSE("Confirm");

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }
        else
        {
            Debug.LogError("ConfirmButton: Failed to find or create LoadingManager.");
        }
    }
}

