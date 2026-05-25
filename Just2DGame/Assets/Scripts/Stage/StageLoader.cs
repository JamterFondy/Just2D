using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class StageLoader : MonoBehaviour
{
    private StageLayout layout;
    public BGManager bgManager;
    public GameObject[] enemyPrefabs;
    public GameObject BossHPBar;


    public string stageName;



    [System.Serializable]
    public class EnemyLayoutEntry
    {
        public string Type;
        public int InsNum;
        public float x;
        public float y;
        public int SpawnNum;
        public float SpawnSpan;
        public float StartDelay;
    }

    [System.Serializable]
    public class StageData
    {
        public string StageName;
        public string Background;
        public string BGM;
    }

    [System.Serializable]
    public class QuestData
    {
        public int BossCount;
        public string MainQuest;
        public string SubQuest;
    }

    [System.Serializable]
    public class StageLayout
    {
        public EnemyLayoutEntry[] enemies;
        public StageData stageData;
        public QuestData questData;
    }

    void Awake()
    {
        bgManager = FindAnyObjectByType<BGManager>();
    }
    void Start()
    {
        LoadJson();
        LoadBackground();
        LoadStageData();
        LoadQuestData();
        LoadLayout();
    }

    void LoadJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("StageLayouts/layout");
        layout = JsonUtility.FromJson<StageLayout>(jsonFile.text);
    }

    void LoadBackground()
    {
        // 背景を差し替える処理
    }

    void LoadStageData()
    {
        if (layout == null || layout.stageData == null)
        {
            Debug.Log("StageLoader: No stage data found in layoutJson.");
            return;
        }

        stageName = layout.stageData.StageName;
        string background = layout.stageData.Background;
        string bgmName = layout.stageData.BGM;


        if (bgManager != null)
        {
            bgManager.PlayBattleBGM(bgmName);
           
        }
        else
        {
            Debug.Log("BGManagerなんかねぇよ");
        }
    }

    void LoadQuestData()
    {
        if (layout == null || layout.questData == null)
        {
            Debug.Log("StageLoader: No quest data found in layoutJson.");
            return;
        }
        int bossCount = layout.questData.BossCount;
        string mainQuest = layout.questData.MainQuest;
        string[] subQuest = layout.questData.SubQuest.Split(';');
        
        StageQuestPannel questPannel = FindAnyObjectByType<StageQuestPannel>();
        if (questPannel != null)
        {
            questPannel.SetQuestText(mainQuest, subQuest);
        }
        else
        {
            Debug.Log("StageQuestPannelなんかねぇよ");
        }

        // ポーズメニューでも確認できるようにしたい。しかし、ポーズメニューは開幕でSetActive(false)にされるため、FindAnyObjectByTypeで取得できない。
    }

    void LoadLayout()
    {
        if (layout == null || layout.enemies == null || layout.enemies.Length == 0)
        {
            Debug.LogWarning("StageLoader: No layout entries found in layoutJson.");
            return;
        }

        // start spawning from InsNum = 1
        StartCoroutine(SpawnEnemySequence(layout, 1));
        
    }


    // JSONファイルのInsNumとスクリプトのinsNumが等号となる行のデータを受けてリスト化し、それを敵スポーンの処理に投げる関数。
    IEnumerator SpawnEnemySequence(StageLayout layout, int insNum)
    {
        // layout内のenemiesを参照し、その内のInsNumが現在のinsNumと等しい行をentriesリストとして上書き。
        // つまりentriesはリストであるが、その中身はJSONの一行分ということになる。今回は(1 * N)の行列のリスト化ということ。
        var entries = layout.enemies
            .Where(e => e != null && e.InsNum == insNum)
            .ToList();


        if (entries.Count == 0)// 今回獲得したentriesにデータが入っていない場合の処理。JSONファイルが正しく記述されていれば処理されることはない。
        {
            // 次の InsNum が存在するか確認。いる場合はinsNum+1をして上記の処理を行う。
            if (layout.enemies.Any(e => e != null && e.InsNum > insNum))
                yield return StartCoroutine(SpawnEnemySequence(layout, insNum + 1));

            yield break;
        }


        // 全エントリのコルーチンを開始
        List<Coroutine> coroutines = new List<Coroutine>();

        // 次の InsNum が存在する場合は通常エネミーとして扱う。
        if (layout.enemies.Any(e => e != null && e.InsNum > insNum))
        {
            foreach (var entry in entries)
                coroutines.Add(StartCoroutine(SpawnEnemyByNumber(entry, insNum, false)));
        }
        else
        {
            Debug.Log("最後の敵だよー");

            // 次の InsNum が存在しない場合は最後のエネミー（ボス）として扱う。
            foreach (var entry in entries)          
                coroutines.Add(StartCoroutine(SpawnEnemyByNumber(entry, insNum, true)));
        }


        // 最大時間だけ待つ（SpawnEnemyByNumber は時間が決まっている）
        float maxWait = entries.Max(e => e.StartDelay + e.SpawnNum * e.SpawnSpan) + 0.1f;
        yield return new WaitForSeconds(maxWait);


        // 次の InsNum へ
        yield return StartCoroutine(SpawnEnemySequence(layout, insNum + 1)); //ToDo　この時にJSOＮの敵構成ファイルの行を取って、次が最終行の敵生成である場合、その敵にボスコンポーネントを付与するようにする。
    }


    // 上の関数からもらったentry(entriesに含まれているデータの一つ一つ)を参考に実際に敵生成を行う関数。
    IEnumerator SpawnEnemyByNumber(EnemyLayoutEntry entry, int currentInsNum, bool isBoss)
    {
        Debug.Log($"スポーン開始: {entry.Type}、位置: ({entry.x}, {entry.y})、開始遅延: {entry.StartDelay}, スポーン数: {entry.SpawnNum}, スポーン間隔: {entry.SpawnSpan}");

        if (entry == null) yield break;


        //以下、敵のタイプに則したプリファブを指定。
        GameObject prefab = null;

        foreach(var enemyPrefab in enemyPrefabs)
        {
            if (entry.Type == enemyPrefab.name)
            {
                prefab = enemyPrefab;
                break;
            }
        }

        if (prefab == null)
        {
            Debug.LogWarning($"StageLoader: Unknown enemy Type '{entry.Type}'");
        }

        if (entry.StartDelay > 0f)
            yield return new WaitForSeconds(entry.StartDelay);

        for (int i = 1; i <= entry.SpawnNum; i++)
        {
            if (prefab != null)
            {
                Vector3 pos = new Vector3(entry.x, entry.y, 0.5f);
                GameObject enemy = Instantiate(prefab, pos, Quaternion.identity);
                if (isBoss) enemy.AddComponent<BossEnemy>();
                if (isBoss) enemy.GetComponent<BossEnemy>().bossHPBar = BossHPBar;
            }

            if (i < entry.SpawnNum)
                yield return new WaitForSeconds(entry.SpawnSpan);
        }

    }
}
