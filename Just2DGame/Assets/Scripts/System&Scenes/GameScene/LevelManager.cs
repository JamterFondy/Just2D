using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class LevelManager : MonoBehaviour
{
    [Tooltip("Reference to a LevelData ScriptableObject that defines waves and optional stage prefab")]
    public LevelData levelData;

    [Tooltip("Automatically start the level on Start() if true")]
    public bool autoStart = true;

    // Optional: you can assign a parent transform for spawned enemies
    public Transform spawnParent;

    public int stageID;

    void Start()
    {
        if (autoStart && levelData != null)
        {
            StartCoroutine(RunLevel());
        }
    }

    public IEnumerator RunLevel()
    {
        if (levelData == null)
            yield break;

        // Instantiate stage prefab if provided
        if (levelData.stagePrefab != null)
        {
            Instantiate(levelData.stagePrefab);
        }

        float levelStart = Time.time;

        // Iterate over waves in order of spawnTime
        levelData.waves.Sort((a, b) => a.spawnTime.CompareTo(b.spawnTime));

        foreach (var wave in levelData.waves)
        {
            float targetTime = levelStart + wave.spawnTime;
            while (Time.time < targetTime)
                yield return null;

            // Start spawning this wave (spawn coroutine so waves can overlap)
            StartCoroutine(SpawnWave(wave));
        }
    }

    IEnumerator SpawnWave(WaveEntry wave)
    {
        if (wave.enemyPrefab == null)
            yield break;

        for (int i = 0; i < Mathf.Max(1, wave.count); i++)
        {
            var pos = (Vector3)wave.spawnPosition;
            var go = Instantiate(wave.enemyPrefab, pos, Quaternion.identity, spawnParent);
            yield return new WaitForSeconds(wave.interval);
        }
    }
}
