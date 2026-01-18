using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("回避設定")]
    [SerializeField] float detectionRadius = 3f;       // 弾を検出して回避を始める半径
    [SerializeField] float avoidanceStrength = 4f;     // 回避ベクトルの強さ
    [SerializeField] string bulletTag = "Bullet";      // 回避対象のタグ（Inspectorで変更可）

    [Header("浮遊（ボブ）動作")]
    [SerializeField] float bobAmplitude = 0.5f;        // 上下振幅（単位: Unity 単位）
    [SerializeField] float bobFrequency = 0.5f;        // 周波数（Hz）

    [Header("基本移動")]
    [SerializeField] float horizontalSpeed = 1f;       // 水平方向の速度（常に正の値を入れる）
    [SerializeField] float directionSwitchInterval = 3f; // 方向を切り替える間隔（秒）

    [Header("画面制限")]
    [SerializeField] Camera cam;

    Vector3 startPosition;
    float omega;
    int horizontalDirection = -1; // 初期は左（-1）。必要なら Inspector で初期値を変える処理を追加してください。

    void Start()
    {
        startPosition = transform.position;
        omega = 2f * Mathf.PI * Mathf.Max(0f, bobFrequency);

        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
                Debug.LogWarning("Main Camera not found. Movement bounds will not be applied.");
        }

        StartCoroutine(DirectionSwitchLoop());
    }

    void Update()
    {
        // --- 回避ベクトル計算（タグ "Bullet" を持つオブジェクト） ---
        Vector3 avoid = Vector3.zero;
        GameObject[] bullets = GameObject.FindGameObjectsWithTag(bulletTag);
        foreach (var b in bullets)
        {
            if (b == null) continue;
            Vector3 toEnemy = transform.position - b.transform.position;
            float dist = toEnemy.magnitude;
            if (dist <= 0.0001f) continue;

            if (dist < detectionRadius)
            {
                float weight = 1f - (dist / detectionRadius);
                avoid += toEnemy.normalized * weight;
            }
        }

        // --- 基本の浮遊（ボブ）と横移動を速度ベースで作成 ---
        Vector3 bobVelocity = Vector3.up * (bobAmplitude * omega * Mathf.Cos(omega * Time.time));
        Vector3 horizVelocity = Vector3.right * horizontalDirection * horizontalSpeed;
        Vector3 baseVelocity = horizVelocity + bobVelocity;

        if (avoid != Vector3.zero)
        {
            baseVelocity += avoid.normalized * avoidanceStrength;
        }

        transform.position += baseVelocity * Time.deltaTime;


        // カメラの右半分内に制限（カメラがある場合）
        if (cam != null)
        {
            float distance = transform.position.z - cam.transform.position.z;

            Vector3 rightBottom = cam.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
            Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
            Vector3 rightMiddle = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));

            float minX = rightMiddle.x;   // ビューポートの x = 0.5 を左端とする
            float maxX = rightBottom.x;   // ビューポートの x = 1 を右端とする
            float minY = rightBottom.y;
            float maxY = rightTop.y;

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }
    }

    IEnumerator DirectionSwitchLoop()
    {
        if (directionSwitchInterval <= 0f)
            yield break;

        while (true)
        {
            yield return new WaitForSeconds(directionSwitchInterval);
            horizontalDirection *= -1;
        }
    }

    // デバッグ：検出半径とボブの中心点を表示
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.up * bobAmplitude, transform.position - Vector3.up * bobAmplitude);
    }
}
