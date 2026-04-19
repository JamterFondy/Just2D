using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveEntry
{
    [Tooltip("Time (seconds) from level start when this wave begins")]
    public float spawnTime = 0f;

    [Tooltip("Prefab to spawn for this wave")]
    public GameObject enemyPrefab;

    [Tooltip("World position to spawn the enemies at")]
    public Vector2 spawnPosition = Vector2.zero;

    [Tooltip("How many enemies to spawn in this wave")]
    public int count = 1;

    [Tooltip("Delay between individual spawns inside this wave")]
    public float interval = 0.1f;
}

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data", order = 100)]
public class LevelData : ScriptableObject
{
    [Tooltip("Optional prefab for stage layout (tilemap / obstacles). Instantiated at level start if set)")]
    public GameObject stagePrefab;

    [Tooltip("Wave entries describing when and what to spawn")]
    public List<WaveEntry> waves = new List<WaveEntry>();
}
