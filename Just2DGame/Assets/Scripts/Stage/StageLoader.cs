using System.Collections;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public StageData stageData;

    public GameObject collarZakoSpeedPrefab;
    public GameObject collarBossPrefab;
    public GameObject spikePrefab;

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
    public class StageLayout
    {
        public EnemyLayoutEntry[] enemies;
    }

    void Start()
    {
        LoadBackground();
        LoadBGM();
        LoadLayout();
    }

    void LoadBackground()
    {
        // 背景を差し替える処理
    }

    void LoadBGM()
    {
        // BGM を差し替える処理
    }

    void LoadLayout()
    {
        var layout = JsonUtility.FromJson<StageLayout>(stageData.layoutJson.text);

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
        // find entries for this insNum
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
        if (entry == null) yield break;

        // determine prefab by Type
        GameObject prefab = null;
        if (entry.Type == "CollarZakoSpeed") prefab = collarZakoSpeedPrefab;
        else if (entry.Type == "CollarBoss") prefab = collarBossPrefab;
        else if (entry.Type == "Spike") prefab = spikePrefab;
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
                Vector2 pos = new Vector2(entry.x, entry.y);
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
