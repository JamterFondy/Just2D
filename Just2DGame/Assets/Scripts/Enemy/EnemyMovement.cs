using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    [Header("回避設定")]
    [SerializeField] float detectionRadius = 3f;
    [SerializeField] float avoidanceStrength = 4f;
    [SerializeField] string bulletTag = "Bullet";

    [Header("浮遊（ボブ）動作")]
    [SerializeField] float bobAmplitude = 0.5f;
    [SerializeField] float bobFrequency = 0.5f;

    [Header("基本移動")]
    [SerializeField] float horizontalSpeed = 1f;
    [SerializeField] float directionSwitchInterval = 3f;

    [Header("画面制限")]
    [SerializeField] Camera cam;

    Vector3 startPosition;
    float omega;
    int horizontalDirection = -1;

    EnemyStatus enemyStatus;

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();

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
        // 拘束中は一切の移動をしない（回避含む）
        if (enemyStatus.currentAffected == Affected.Restricted) return;


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

        Vector3 bobVelocity = Vector3.up * (bobAmplitude * omega * Mathf.Cos(omega * Time.time));
        Vector3 horizVelocity = Vector3.right * horizontalDirection * horizontalSpeed;
        Vector3 baseVelocity = horizVelocity + bobVelocity;

        if (avoid != Vector3.zero)
        {
            baseVelocity += avoid.normalized * avoidanceStrength;
        }

        transform.position += baseVelocity * Time.deltaTime;

        if (cam != null)
        {
            float distance = transform.position.z - cam.transform.position.z;

            Vector3 rightBottom = cam.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
            Vector3 rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, distance));
            Vector3 rightMiddle = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));

            float minX = rightMiddle.x;
            float maxX = rightBottom.x;
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

    // 外部から拘束を適用するための API
    //public void ApplyRestraint(float duration)
    //{
    //  if (duration <= 0f) return;
    //    isRestrained = true;
        // 新たに適用された拘束は現在の残り時間を上書きせず、長い方を採用
    //    restraintTimer = Mathf.Max(restraintTimer, duration);
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + Vector3.up * bobAmplitude, transform.position - Vector3.up * bobAmplitude);
    }
}
