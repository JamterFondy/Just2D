using System.IO;
using UnityEngine;

public class NextStageLoader : MonoBehaviour
{

    private StageDataList stageDataList;

    [System.Serializable]
    public class StageData
    {
        public string StageName;
        public string Background;
        public string BGM;
    }

    [System.Serializable]
    public class StageDataList
    {
        public StageData stageData;
    }

    string currentStageName;
    int currentStageNum;
    int nextStageNum;

    public bool ChangeLayout_NextStage(bool IsLoadDone)
    {
        IsLoadDone = false;

        Debug.Log("動いているよー");

        TextAsset layout_jsonFile = Resources.Load<TextAsset>("StageLayouts/layout");
        stageDataList = JsonUtility.FromJson<StageDataList>(layout_jsonFile.text);

        if (layout_jsonFile != null)
        {
            Debug.Log("layout.jsonは見つけたよー");

            // 現在のステージ名から数字部分を取り出し、次のステージの名前を生成する
            currentStageName = stageDataList.stageData.StageName.Replace("-", "");

            currentStageNum = int.Parse(currentStageName);

            // 10で割り切れるステージ番号は生成しない
            if ((currentStageNum + 1) % 10 == 0)
            {
                nextStageNum = currentStageNum + 2;
            }
            else
            {
                nextStageNum = currentStageNum + 1;
            }

            Debug.Log($"次のステージは layout_{nextStageNum}.json");

            TextAsset nextLayout_jsonFile = Resources.Load<TextAsset>($"StageLayouts/layout_{nextStageNum}");
            var nextLayout_path = File.ReadAllText(Path.Combine(Application.dataPath, "Resources", "StageLayouts", $"layout_{nextStageNum}.json"));

            if (nextLayout_jsonFile != null)
            {
                // layout.JSONを次のステージの内容で上書きする
                string currentLayout_path = Path.Combine(Application.dataPath, "Resources", "StageLayouts", "layout.JSON");
                File.WriteAllText(currentLayout_path, nextLayout_path);
                Debug.Log($"NextStageLoader: Replaced layout.JSON with layout_{nextStageNum}.json");

#if UNITY_EDITOR
                // Refresh the AssetDatabase so Unity recognizes the changed TextAsset in the Editor
                UnityEditor.AssetDatabase.Refresh();
#endif

                IsLoadDone = true;
            }
            else
            {
                Debug.LogError("Failed to load layout JSON file.");
            }
        }

        return IsLoadDone;
    }
}
