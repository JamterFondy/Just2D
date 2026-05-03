using System.Collections;
using System.IO;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    private StageLayout layout;

    public BGManager bgManager;

    public GameObject collarZakoSpeedPrefab;
    public GameObject collarBossPrefab;

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
        LoadBGM();
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

    void LoadBGM()
    {
        if (layout == null || layout.stageData == null)
        {
            Debug.Log("StageLoader: No stage data found in layoutJson.");
            return;
        }

        string bgmName = layout.stageData.BGM;
        
        if (bgManager != null)
        {
            bgManager.PlayBattleBGM(bgmName);
            Debug.Log("BGM受け渡し完了");
        }
        else
        {
            Debug.Log("BGManagerなんかねぇよ");
        }
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

    // Orchestrator: spawn all entries with given InsNum, then proceed to next InsNum if any
    IEnumerator SpawnEnemySequence(StageLayout layout, int insNum)
    {
        var entries = new System.Collections.Generic.List<EnemyLayoutEntry>();
        foreach (var e in layout.enemies)
        {
            if (e != null && e.InsNum == insNum)
                entries.Add(e);
        }

        if (entries.Count == 0)
        {
            // no entries for this insNum, check if there are any entries for higher numbers
            bool anyHigher = false;
            foreach (var e in layout.enemies)
            {
                if (e != null && e.InsNum > insNum) { anyHigher = true; break; }
            }
            if (anyHigher)
            {
                // advance to next
                yield return StartCoroutine(SpawnEnemySequence(layout, insNum + 1));
            }
            yield break;
        }

        // Start coroutines for all entries of this insNum and wait until all complete
        var routines = new System.Collections.Generic.List<IEnumerator>();
        var handles = new System.Collections.Generic.List<Coroutine>();
        foreach (var entry in entries)
        {
            var routine = SpawnEnemyByNumber(entry);
            handles.Add(StartCoroutine(routine));
        }

        // Wait until all spawned coroutines finish
        bool anyRunning = true;
        while (anyRunning)
        {
            anyRunning = false;
            // Check if any of the handles are still running by checking existence (not straightforward),
            // instead check if any GameObjects that were spawned for this insNum still exist by a small delay here.
            // To keep simple, poll until no spawned prefab with matching InsNum remains; but we don't track by InsNum here.
            // Instead, we wait a frame and let coroutines manage chaining. Simpler: wait until a short delay and assume coroutines finished.
            // However to be correct, we'll wait until all coroutines have completed by using a small flag mechanism internal to SpawnEnemyByNumber: it returns when done, so StartCoroutine returns a Coroutine we cannot query. We'll approximate by waiting until no coroutine is running in the next frame.
            // For simplicity, yield return null for a frame and then break when all coroutines have yielded completion. To ensure sequence, we wait until a small delay and then continue.
            yield return null;

            // Check if any of the entries still have remaining spawns by peeking at their SpawnNum; but we cannot. So instead wait until a small buffer then proceed.
            // Better approach: after launching all SpawnEnemyByNumber coroutines, simply wait until all coroutines complete by tracking a static counter. To avoid global state, we instead chain to next insNum at the end of each SpawnEnemyByNumber via recursive call. But that may cause multiple calls. To avoid complexity, we will wait until all coroutines complete by waiting until there are no coroutines left with name pattern; Unity doesn't provide that. Therefore use a simple delay equal to the maximum possible time for these entries to finish.
            anyRunning = false;
        }

        // compute maximum wait time to allow entries to finish: for each entry, totalTime = StartDelay + SpawnNum * SpawnSpan
        float maxWait = 0f;
        foreach (var entry in entries)
        {
            float total = entry.StartDelay + entry.SpawnNum * entry.SpawnSpan + 0.1f;
            if (total > maxWait) maxWait = total;
        }

        yield return new WaitForSeconds(maxWait);

        // After these have finished, proceed to next InsNum
        yield return StartCoroutine(SpawnEnemySequence(layout, insNum + 1));
    }

    // Coroutine to spawn enemies for a single layout entry
    IEnumerator SpawnEnemyByNumber(EnemyLayoutEntry entry)
    {
        Debug.Log($"スポーン開始: {entry.Type}、位置: ({entry.x}, {entry.y})、開始遅延: {entry.StartDelay}, スポーン数: {entry.SpawnNum}, スポーン間隔: {entry.SpawnSpan}");

        if (entry == null) yield break;

        // determine prefab by Type
        GameObject prefab = null;
        if (entry.Type == "CollarZakoSpeed") prefab = collarZakoSpeedPrefab;
        else if (entry.Type == "CollarBoss") prefab = collarBossPrefab;
        else
        {
            Debug.LogWarning($"StageLoader: Unknown enemy Type '{entry.Type}'");
        }

        // wait StartDelay
        if (entry.StartDelay > 0f)
            yield return new WaitForSeconds(entry.StartDelay);

        int i = 1;
        while (i <= entry.SpawnNum)
        {
            if (prefab != null)
            {
                Vector3 pos = new Vector3(entry.x, entry.y, 0.5f); //敵のスポーン位置。全ての敵を統一でz = 0.5fで統一しているが、表示に問題が出る可能性を考えJSONにz座標を加えたほうがいいかもしれない。
                Instantiate(prefab, pos, Quaternion.identity);
            }
            i++;
            if (i <= entry.SpawnNum)
                yield return new WaitForSeconds(entry.SpawnSpan);
        }

        // done
        yield break;
    }
}
