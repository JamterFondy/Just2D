using UnityEngine;
using System.Collections;

public class SpawnELineBullet : MonoBehaviour
{
    [Header("生成設定")]
    [SerializeField] GameObject prefab;            // 生成するプレハブ
    [SerializeField] float startDelay = 2f;        // 処理開始から何秒後に生成を開始するか
    [SerializeField] int perDirectionCount = 8;    // 各方向に生成する数
    [SerializeField] float perSpawnInterval = 0.1f;// 各生成間の間隔（秒）
    [SerializeField] float spawnDistance = -1f;    // 生成位置 = transform.position + dir * spawnDistance

    Coroutine roopCoroutine;
    Coroutine spawnCoroutine;

    void Start()
    {
        // 自動で開始
        if (prefab == null)
        {
            Debug.LogWarning("SpawnELineBullet: prefab が未設定です。");
            return;
        }

        roopCoroutine = StartCoroutine(RoopSpawn());
    }

    IEnumerator RoopSpawn()
    {
        // 指定遅延
        yield return new WaitForSeconds(Mathf.Max(0f, startDelay));

        spawnCoroutine = StartCoroutine(DelayedSpawnSequence());
    }


    IEnumerator DelayedSpawnSequence()
    {

        // 時計の目盛りで 12時を 90deg とする（同じ定義を他スクリプトと合わせる）
        float startAngle = 0f;
        float step = 360f / 12f;

        // 生成する時刻（目盛りインデックス）: 8,9,10
        int[] indices = new int[] {7, 8, 9, 10, 11 };

        // 各方向について順に生成（各方向ごとに perDirectionCount 個、間隔 perSpawnInterval）
        foreach (int idx in indices)
        {
            float angle = startAngle - idx * step; // 時計回りにマイナス方向
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f).normalized;

            for (int i = 0; i < perDirectionCount; i++)
            {
                Vector3 spawnPos = transform.position + dir * spawnDistance;
                // z は元の z を保つ
                spawnPos.y = transform.position.y;
                spawnPos.z = transform.position.z;

                // 生成時の向きは外向き（角度を与える）。不要なら Quaternion.identity に変更可
                Quaternion rot = Quaternion.Euler(0f, 0f, angle);

                Instantiate(prefab, spawnPos, rot);

                yield return new WaitForSeconds(Mathf.Max(0f, perSpawnInterval));
            }
        }

        spawnCoroutine = null;

        roopCoroutine = StartCoroutine(RoopSpawn());
    }

    // 外部から単発で呼びたい場合の API（必要なら利用）
    public void SpawnNowOnce()
    {
        if (prefab == null) { Debug.LogWarning("SpawnELineBullet: prefab が未設定です。"); return; }
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnNowCoroutine());
    }

    IEnumerator SpawnNowCoroutine()
    {
        float startAngle = 0f;
        float step = 360f / 12f;
        int[] indices = new int[] { 8, 9, 10 };

        foreach (int idx in indices)
        {
            float angle = startAngle - idx * step;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f).normalized;

            for (int i = 0; i < perDirectionCount; i++)
            {
                Vector3 spawnPos = transform.position + dir * spawnDistance;
                spawnPos.z = transform.position.z;
                Quaternion rot = Quaternion.Euler(0f, 0f, angle);
                Instantiate(prefab, spawnPos, rot);
                yield return new WaitForSeconds(Mathf.Max(0f, perSpawnInterval));
            }
        }

        spawnCoroutine = null;
    }

    void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }
}