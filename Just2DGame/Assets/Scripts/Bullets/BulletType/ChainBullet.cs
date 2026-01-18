using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainBullet : MonoBehaviour
{
    Camera cam;

    [SerializeField] ESC esc;

    [SerializeField] GameObject prefab1;     // 上下に生成するオブジェクト（1つずつ）
    [SerializeField] GameObject prefab2;     // 線上に間隔で生成するオブジェクト（途中用）
    [SerializeField] GameObject prefab2Final; // 線上の最後に生成する別プレハブ（最終弾、拘束を付与）

    [SerializeField] float verticalOffset = 0.8f; // prefab1 の上下オフセット
    [SerializeField] float spacing = 0.8f;     // prefab2 の間隔
    [SerializeField] float spawnInterval = 0.02f; // prefab2 を生成する時間間隔（秒）

    [SerializeField] float moveDelayAfterSpawn = 1f; // 最後の生成から何秒後に移動開始するか
    [SerializeField] float moveSpeed = 30f; // prefab2 の移動速度
    [SerializeField] float leftCrickCoolTime = 3f; // 左クリックのクールタイム

    // 追加: prefab2 の向きがスプライトのデフォルト向きと合わない場合に調整するオフセット（度）
    [SerializeField] float prefab2RotationOffset = 0f;

    // 追加: 最後に生成される prefab2 に付与する拘束の継続時間（秒）
    [Header("Restraint (最後の prefab2 に付与)")]
    [SerializeField] float restraintDuration = 1.5f;
    [SerializeField] bool restraintDestroyBulletOnTrigger = false;

    Coroutine topCoroutine;
    Coroutine bottomCoroutine;
    Coroutine moveCoroutine;

    bool LeftCrickCoolTime = false;

    class MovingInstance
    {
        public GameObject obj;
        public Vector3 dir;
    }

    List<MovingInstance> spawnedPrefab2 = new List<MovingInstance>();
    int completedSpawnCoroutines = 0; // top/bottom 両方の生成コルーチンが終わったか判定

    void Start()
    {
        if (esc == null)
        {
            esc = FindObjectOfType<ESC>();
        }

        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("Main Camera not found. Movement bounds will not be applied.");
    }

    void Update()
    {
        if (!LeftCrickCoolTime && Input.GetMouseButtonDown(0) && !esc.isPaused)
        {
            LeftCrickCoolTime = true;

            if (cam == null)
            {
                Debug.LogWarning("Main Camera not found. Cannot compute mouse world position.");
                return;
            }

            if (topCoroutine != null) { StopCoroutine(topCoroutine); topCoroutine = null; }
            if (bottomCoroutine != null) { StopCoroutine(bottomCoroutine); bottomCoroutine = null; }
            if (moveCoroutine != null) { StopCoroutine(moveCoroutine); moveCoroutine = null; }

            spawnedPrefab2.Clear();
            completedSpawnCoroutines = 0;

            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = transform.position.z - cam.transform.position.z;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = transform.position.z;

            Vector3 myPos = transform.position;

            Vector3 topPos = new Vector3(myPos.x, myPos.y + verticalOffset, myPos.z);
            Vector3 bottomPos = new Vector3(myPos.x, myPos.y - verticalOffset, myPos.z);

            if (prefab1 != null)
            {
                Instantiate(prefab1, topPos, Quaternion.identity);
                Instantiate(prefab1, bottomPos, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("prefab1 is not assigned.");
            }

            if (prefab2 != null)
            {
                if (topCoroutine != null) StopCoroutine(topCoroutine);
                if (bottomCoroutine != null) StopCoroutine(bottomCoroutine);

                topCoroutine = StartCoroutine(CreateLineInstancesCoroutine(topPos, mouseWorld, prefab2, spacing, spawnInterval, () => { topCoroutine = null; OnSpawnCoroutineCompleted(); }));
                bottomCoroutine = StartCoroutine(CreateLineInstancesCoroutine(bottomPos, mouseWorld, prefab2, spacing, spawnInterval, () => { bottomCoroutine = null; OnSpawnCoroutineCompleted(); }));
            }
            else
            {
                Debug.LogWarning("prefab2 is not assigned.");
            }
        }
    }

    void OnSpawnCoroutineCompleted()
    {
        completedSpawnCoroutines++;
        if (completedSpawnCoroutines >= 2)
        {
            if (moveCoroutine != null) { StopCoroutine(moveCoroutine); moveCoroutine = null; }
            moveCoroutine = StartCoroutine(DelayThenMoveCoroutine(moveDelayAfterSpawn, moveSpeed, leftCrickCoolTime));
            StartCoroutine(CoolTimeL(leftCrickCoolTime));
        }
    }

    // start -> end 間に spacing 間隔で prefab を生成するが、生成は spawnInterval 秒ごとに行う。
    // 途中は prefab（prefab2）を使い、最終位置には prefab2Final を使う（未設定なら prefab2 を使う）
    IEnumerator CreateLineInstancesCoroutine(Vector3 start, Vector3 end, GameObject prefab, float spacing, float spawnInterval, System.Action onComplete)
    {
        Vector3 delta = end - start;
        float distance = delta.magnitude;
        if (distance <= 0.0001f)
        {
            Vector3 toTarget = end - start;
            float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle + prefab2RotationOffset);

            // 最終は prefab2Final があればそちらを使う
            GameObject lastPrefab = prefab2Final != null ? prefab2Final : prefab;
            var go = Instantiate(lastPrefab, end, rot);
            spawnedPrefab2.Add(new MovingInstance { obj = go, dir = (end - start).normalized });

            // 最後の生成なので拘束属性を追加（最後専用）
            TryAddRestraintToFinal(go);

            onComplete?.Invoke();
            yield break;
        }

        Vector3 dir = delta.normalized;

        int step = 1;

        while (true)
        {
            float nextDist = step * spacing;
            if (nextDist < distance)
            {
                Vector3 pos = start + dir * nextDist;
                pos.z = start.z;

                Vector3 toTarget = end - pos;
                float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0f, 0f, angle + prefab2RotationOffset);

                var go = Instantiate(prefab, pos, rot);
                spawnedPrefab2.Add(new MovingInstance { obj = go, dir = dir });
                step++;
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                // 最終位置には別プレハブ（prefab2Final）を生成する
                Vector3 posFinal = end;
                // 最終の向きは start->end の方向（dir）を使う：これでマウス方向を向かせられます
                float angleFinal = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Quaternion rotFinal = Quaternion.Euler(0f, 0f, angleFinal + prefab2RotationOffset);

                GameObject lastPrefab = prefab2Final != null ? prefab2Final : prefab;
                var go = Instantiate(lastPrefab, end, rotFinal);
                spawnedPrefab2.Add(new MovingInstance { obj = go, dir = dir });

                // 最終位置に生成した prefab2Final（または fallback の prefab）に拘束属性を追加（最後専用）
                TryAddRestraintToFinal(go);

                break;
            }
        }

        onComplete?.Invoke();
    }

    // 最終弾専用に拘束コンポーネントを付与する（既に付いていればパラメータをセット）
    void TryAddRestraintToFinal(GameObject go)
    {
        if (go == null) return;

        var rb = go.GetComponent<RestraintBullet>();
        if (rb == null)
            rb = go.AddComponent<RestraintBullet>();

        rb.restraintDuration = restraintDuration;
        rb.destroyOnTrigger = restraintDestroyBulletOnTrigger;
    }

    IEnumerator DelayThenMoveCoroutine(float delay, float moveSpeed, float cooltime)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            spawnedPrefab2.RemoveAll(mi => mi == null || mi.obj == null);

            if (spawnedPrefab2.Count == 0)
            {
                moveCoroutine = null;
                yield break;
            }

            float deltaTime = Time.deltaTime;
            for (int i = 0; i < spawnedPrefab2.Count; i++)
            {
                var mi = spawnedPrefab2[i];
                if (mi == null || mi.obj == null) continue;
                mi.obj.transform.position += mi.dir * moveSpeed * deltaTime;
            }

            yield return null;
        }
    }

    IEnumerator CoolTimeL(float cooltime)
    {
        yield return new WaitForSeconds(cooltime);

        LeftCrickCoolTime = false;
    }
}