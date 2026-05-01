using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/StageData")]
public class StageData : ScriptableObject
{
    public string stageId;
    public string sceneName = "GameScene"; // 全ステージ共通
    public TextAsset layoutJson;           // 敵やギミックの配置データ(JSON)
    public Sprite background;
    public AudioClip bgm;
}
