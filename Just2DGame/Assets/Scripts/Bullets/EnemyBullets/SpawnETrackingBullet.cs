using UnityEngine;
using System.Collections;

public class SpawnETrackingBullet : MonoBehaviour
{
    [Header("生成設定")]
    [SerializeField] GameObject prefab;        // 生成するプレハブ
    [SerializeField] float radius = 1.5f;      // 中心からの距離
    [SerializeField] int directions = 12;      // 方向数（時計の数字の位置 = 12）
    [SerializeField] float interval = 0.05f;   // 1つずつ生成する間隔（秒）
    [SerializeField] bool spawnOnStart = true; // Start() で自動生成するか

    [Header("繰り返し設定")]
    [SerializeField] bool repeat = true;       // 生成シーケンスを繰り返すか
    [SerializeField] float repeatInterval = 5f;// シーケンス間の待ち時間（秒）

    bool isSpawning = false;
    Coroutine repeatCoroutine;

    void Start()
    {
        if (!spawnOnStart) return;

        if (prefab == null)
        {
            Debug.LogWarning("EnemyTrackingBullet: prefab が未設定です。");
            return;
        }

        if (repeat)
        {
            repeatCoroutine = StartCoroutine(RepeatSpawnLoop());
        }
        else
        {
            StartCoroutine(SpawnAroundCoroutine());
        }
    }

    // 公開 API：外部から生成を開始したい場合に呼ぶ（単発）
    public void SpawnOnce()
    {
        if (prefab == null)
        {
            Debug.LogWarning("EnemyTrackingBullet: prefab が未設定です。");
            return;
        }
        if (!isSpawning)
            StartCoroutine(SpawnAroundCoroutine());
    }

    IEnumerator RepeatSpawnLoop()
    {
        while (true)
        {
            yield return StartCoroutine(SpawnAroundCoroutine());
            yield return new WaitForSeconds(Mathf.Max(0f, repeatInterval));
        }
    }

    IEnumerator SpawnAroundCoroutine()
    {
        if (isSpawning) yield break;
        isSpawning = true;

        // 12時を起点（上）として時計回りに生成する
        float startAngle = 90f;
        float step = 360f / Mathf.Max(1, directions);

        for (int i = 0; i < directions; i++)
        {
            float angle = startAngle - i * step; // 時計回りに減算
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            Vector3 spawnPos = transform.position + dir * radius;

            // プレハブの向きを外向きに合わせる（必要なければ Quaternion.identity に変更可）
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);

            Instantiate(prefab, spawnPos, rot);

            yield return new WaitForSeconds(Mathf.Max(0f, interval));
        }

        isSpawning = false;
    }

    void OnDestroy()
    {
        if (repeatCoroutine != null)
            StopCoroutine(repeatCoroutine);
    }
}