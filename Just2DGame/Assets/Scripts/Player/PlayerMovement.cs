using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed = 6f;
    Camera cam;

    [SerializeField] GameObject prefab1;     // 上下に生成するオブジェクト（1つずつ）
    [SerializeField] GameObject prefab2;     // 線上に間隔で生成するオブジェクト
    [SerializeField] float verticalOffset = 0.8f;　// prefab1 の上下オフセット
    [SerializeField] float spacing = 0.8f;     // prefab2 の間隔
    [SerializeField] float spawnInterval = 0.1f; // prefab2 を生成する時間間隔（秒）

    [SerializeField] float moveDelayAfterSpawn = 1f; // 最後の生成から何秒後に移動開始するか
    [SerializeField] float moveSpeed = 10f; // prefab2 の移動速度
    [SerializeField] float leftCrickCoolTime = 5f; // 左クリックのクールタイム

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
        // 入力（W/A/S/D）
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) input.y += 1f;
        if (Input.GetKey(KeyCode.S)) input.y -= 1f;
        if (Input.GetKey(KeyCode.D)) input.x += 1f;
        if (Input.GetKey(KeyCode.A)) input.x -= 1f;
        if (input.sqrMagnitude > 1f) input.Normalize();

        // 移動
        Vector3 delta = new Vector3(input.x, input.y, 0f) * speed * Time.deltaTime;
        transform.position += delta;

        // カメラの左半分矩形内にクランプ（カメラがある場合）
        if (cam != null)
        {

            // カメラとプレイヤーのZ差を指定して正しいワールド座標を得る
            float distance = transform.position.z - cam.transform.position.z;

            // ビューポートからワールド座標を計算
            Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
            Vector3 rightMiddle = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            Vector3 top = cam.ViewportToWorldPoint(new Vector3(0f, 1f, distance));

            float minX = leftBottom.x;
            float maxX = rightMiddle.x;    // カメラ幅の中央（左半分の右端）
            float minY = leftBottom.y;
            float maxY = top.y;

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }


        // スペースキーで指定処理を実行
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

            // ① マウスポインタのワールド座標を取得
            Vector3 mouseScreen = Input.mousePosition;
            // カメラとプレイヤーのZ差を使って正しいワールド座標を得る
            mouseScreen.z = transform.position.z - cam.transform.position.z;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
            // z はプレイヤーと同じにしておく
            mouseWorld.z = transform.position.z;

            // ② 現在のオブジェクトの座標を取得
            Vector3 myPos = transform.position;

            // ③ 現在のオブジェクトの上下 verticalOffset に prefab1 を生成
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


            // ④ prefab1（start） -> マウスポインタ（end）に向けて、0.1秒ごとに prefab2 を生成（spacing ごとに移動）
            if (prefab2 != null)
            {
                // 既存のコルーチンがあれば停止してから開始（連続実行を防ぐ）
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

    // spawn コルーチンが1つ終了するたびに呼ばれる
    void OnSpawnCoroutineCompleted()
    {
        completedSpawnCoroutines++;
        // 上下両方のコルーチンが完了したら、move を開始する（遅延付き）
        if (completedSpawnCoroutines >= 2)
        {
            // moveCoroutine が既にあれば停止してから再開（安全措置）
            if (moveCoroutine != null) { StopCoroutine(moveCoroutine); moveCoroutine = null; }
            moveCoroutine = StartCoroutine(DelayThenMoveCoroutine(moveDelayAfterSpawn, moveSpeed, leftCrickCoolTime));
            StartCoroutine(CoolTimeL(leftCrickCoolTime));
        }
    }

    // start -> end 間に spacing 間隔で prefab を生成するが、生成は spawnInterval 秒ごとに行う。
    // 生成した prefab2 は spawnedPrefab2 リストに登録する。最後は必ず end に到達するように end にインスタンスを作る。
    IEnumerator CreateLineInstancesCoroutine(Vector3 start, Vector3 end, GameObject prefab, float spacing, float spawnInterval, System.Action onComplete)
    {
        Vector3 delta = end - start;
        float distance = delta.magnitude;
        if (distance <= 0.0001f)
        {
            // ほとんど同じ位置なら end に1つ生成して終了
            var go = Instantiate(prefab, end, Quaternion.identity);
            spawnedPrefab2.Add(new MovingInstance { obj = go, dir = (end - start).normalized });
            onComplete?.Invoke();
            yield break;
        }

        Vector3 dir = delta.normalized;

        int step = 1;

        // step=1 から始め、start + dir * (step * spacing) に生成していく
        while (true)
        {
            float nextDist = step * spacing;
            if (nextDist < distance)
            {
                Vector3 pos = start + dir * nextDist;
                pos.z = start.z;
                var go = Instantiate(prefab, pos, Quaternion.identity);
                // 生成した prefab2 を追跡リストに登録（進行方向は start->end の方向）
                spawnedPrefab2.Add(new MovingInstance { obj = go, dir = dir });
                step++;
                // 指定の間隔だけ待つ
                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                // 最終位置（マウスポインタ）に到達させる
                var go = Instantiate(prefab, end, Quaternion.identity);
                spawnedPrefab2.Add(new MovingInstance { obj = go, dir = dir });
                break;
            }
        }

        onComplete?.Invoke();
    }

    // 最後の生成から delay 秒待って、spawnedPrefab2 にある全てのオブジェクトを
    // 各自の dir に沿って毎フレーム移動させる（速度 moveSpeed）。移動は無期限に続く（必要なら停止条件を追加してください）。
    IEnumerator DelayThenMoveCoroutine(float delay, float moveSpeed, float cooltime)
    {
        yield return new WaitForSeconds(delay);

        // 移動ループ
        while (true)
        {
            // null や破棄済みオブジェクトを取り除く
            spawnedPrefab2.RemoveAll(mi => mi == null || mi.obj == null);

            // すべて消えていたら終了
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
