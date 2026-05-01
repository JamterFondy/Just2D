using System.IO;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{

    public int stageNum;

    // Replace the content of layout.JSON under Assets/Scripts/Stage with the file named layout_{stageNum}.* (json)
    public void DetermineStageLayout()
    {
        string stageFolder = Path.Combine(Application.dataPath, "Resources", "StageLayouts"); //ToDo stageFolderはResourcesフォルダ内に入れるべきかもしれないが、現状はこのまま
        if (!Directory.Exists(stageFolder))
        {
            Debug.LogWarning($"StageSelectManager: stage folder not found: {stageFolder}");
            return;
        }

        // look for layout_{stageNum}.* (case-insensitive) with .json extension
        string searchPattern = $"layout_{stageNum}.*";
        var files = Directory.GetFiles(stageFolder, searchPattern, SearchOption.TopDirectoryOnly);

        string sourcePath = null;
        foreach (var f in files)
        {
            var ext = Path.GetExtension(f).ToLowerInvariant();
            if (ext == ".json")
            {
                sourcePath = f;
                break;
            }
        }

        if (sourcePath == null)
        {
            Debug.LogWarning($"StageSelectManager: layout file for stage {stageNum} not found in {stageFolder} (expected pattern layout_{stageNum}.json)");
            return;
        }

        // target layout file name (use layout.JSON as requested)
        string targetPath = Path.Combine(stageFolder, "layout.JSON");

        try
        {
            var content = File.ReadAllText(sourcePath);
            File.WriteAllText(targetPath, content);
            Debug.Log($"StageSelectManager: Replaced layout.JSON with {Path.GetFileName(sourcePath)}");

#if UNITY_EDITOR
            // Refresh the AssetDatabase so Unity recognizes the changed TextAsset in the Editor
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"StageSelectManager: Failed to copy layout file: {ex}");
        }
    }
}
