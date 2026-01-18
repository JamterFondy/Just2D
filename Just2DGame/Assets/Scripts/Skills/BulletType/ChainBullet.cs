using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChainBullet : MonoBehaviour
{
    Camera cam;

    [SerializeField] GameObject prefab1;     // 上下に生成するオブジェクト（1つずつ）
    [SerializeField] GameObject prefab2;     // 線上に間隔で生成するオブジェクト

    [SerializeField] float verticalOffset = 0.8f;　// prefab1 の上下オフセット
    [SerializeField] float spacing = 0.8f;     // prefab2 の間隔
    [SerializeField] float spawnInterval = 0.02f; // prefab2 を生成する時間間隔（秒）

    [SerializeField] float moveDelayAfterSpawn = 1f; // 最後の生成から何秒後に移動開始するか
    [SerializeField] float moveSpeed = 30f; // prefab2 の移動速度
    [SerializeField] float leftCrickCoolTime = 3f; // 左クリックのクールタイム

    // 追加: prefab2 の向きがスプライトのデフォルト向きと合わない場合に調整するオフセット（度）
    [SerializeField] float prefab2RotationOffset = 0f;

    // 追加: 最後に生成される prefab2 に付与する拘束の継続時間（秒）
    [Header("Restraint (最後の prefab2 に付与)")]
    [SerializeField] float restraintDuration = 2f;
    [SerializeField] bool restraintDestroyBulletOnTrigger = true;

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
        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("Main Camera not found. Movement bounds will not be applied.");
    }

    void Update()
    {
        if (!LeftCrickCoolTime && Input.GetMouseButtonDown(0))
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

    IEnumerator CreateLineInstancesCoroutine(Vector3 start, Vector3 end, GameObject prefab, float spacing, float spawnInterval, System.Action onComplete)
    {
        Vector3 delta = end - start;
        float distance = delta.magnitude;
        if (distance <= 0.0001f)
        {
            Vector3 toTarget = end - start;
            float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle + prefab2RotationOffset);

            var go = Instantiate(prefab, end, rot);
            spawnedPrefab2.Add(new MovingInstance { obj = go, dir = (end - start).normalized });

            // 最後の生成なので拘束属性を追加
            TryAddRestraint(go);

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
                Vector3 posFinal = end;
                Vector3 toTargetFinal = end - posFinal;
                float angleFinal = Mathf.Atan2(toTargetFinal.y, toTargetFinal.x) * Mathf.Rad2Deg;
                Quaternion rotFinal = Quaternion.Euler(0f, 0f, angleFinal + prefab2RotationOffset);

                var go = Instantiate(prefab, end, rotFinal);
                spawnedPrefab2.Add(new MovingInstance { obj = go, dir = dir });

                // 最終位置に生成した prefab2 に拘束属性を追加
                TryAddRestraint(go);

                break;
            }
        }

        onComplete?.Invoke();
    }

    void TryAddRestraint(GameObject go)
    {
        if (go == null) return;

        // 既にコンポーネントが付いていなければ追加してパラメータをセット
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
