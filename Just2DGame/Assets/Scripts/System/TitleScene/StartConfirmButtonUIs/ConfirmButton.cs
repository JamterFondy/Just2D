using System.IO;
using UnityEngine;

public class ConfirmButton : MonoBehaviour
{
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClick()
    {
        // 1) Delete all JSON files in persistentDataPath
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

        // 2) Reset PlayerPrefs except MasterVolume, BGMVolume, SEVolume
        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgm = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float se = PlayerPrefs.GetFloat("SEVolume", 1f);

        PlayerPrefs.DeleteAll();

        PlayerPrefs.SetFloat("MasterVolume", master);
        PlayerPrefs.SetFloat("BGMVolume", bgm);
        PlayerPrefs.SetFloat("SEVolume", se);
        PlayerPrefs.Save();

        // 3) Start loading via LoadingManager
        var loadingManager = FindObjectOfType<LoadingManager>();
        if (loadingManager == null)
        {
            // Create a LoadingManager if none exists so coroutine can run
            var go = new GameObject("LoadingManager");
            loadingManager = go.AddComponent<LoadingManager>();
        }

        if (loadingManager != null)
        {
            uiManager.currentState = UIState.HomeDefault;

            loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "HomeScene"));
        }
        else
        {
            Debug.LogError("ConfirmButton: Failed to find or create LoadingManager.");
        }
    }
}

