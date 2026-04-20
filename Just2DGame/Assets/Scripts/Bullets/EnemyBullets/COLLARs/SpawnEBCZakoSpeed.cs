using UnityEngine;
using System.Collections;

public class SpawnEBCZakoSpeed : MonoBehaviour
{
    [Header("生成設定")]
    [SerializeField] GameObject prefab;      // 生成するプレハブ
    [SerializeField] float spawnInterval = 1f; // 生成間隔（秒）

    Coroutine spawnCoroutine;

    void Start()
    {
        if (prefab == null)
        {
            Debug.LogWarning("SpawnEBCZakoSpeed: prefab が未設定です。");
            return;
        }
        spawnCoroutine = StartCoroutine(SpawnLeftRoutine());
    }

    IEnumerator SpawnLeftRoutine()
    {
        while (true)
        {
            // 左方向（ローカル座標での左）
            Vector3 leftDir = -transform.right;
            Vector3 spawnPos = transform.position + leftDir;

            // 親の回転を取得
            Quaternion parentRot = transform.rotation;
            // 親の回転に90度加算（Z軸回転）
            Quaternion spawnRot = parentRot * Quaternion.Euler(0, 0, 90);

            Instantiate(prefab, spawnPos, spawnRot);

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void OnDestroy()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }
}