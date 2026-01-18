using UnityEngine;
using System.Collections;

public class ETrackingBullet : MonoBehaviour
{
    [Header("追尾設定")]
    [SerializeField] float initialDelay = 1f;      // 追尾開始までの遅延（秒）
    [SerializeField] float homingDuration = 2f;    // 追尾継続時間（秒）
    [SerializeField] float speed = 8f;             // 移動速度
    [SerializeField] float rotationSpeed = 720f;   // 回転速度（度/秒）

    Transform target;
    bool isHoming = false;
    Coroutine homingCoroutine;

    void Start()
    {
        // コルーチンで遅延→追尾→直進の流れを制御
        homingCoroutine = StartCoroutine(HomingFlow());
    }

    void Update()
    {
        // 追尾中はコルーチンで移動を行うためここでは isHoming == false のときのみ直進移動を行う
        if (!isHoming)
        {
            // transform.right を「前方向」として直進
            transform.position += transform.right * speed * Time.deltaTime;
        }
    }

    IEnumerator HomingFlow()
    {
        // 初期遅延
        yield return new WaitForSeconds(Mathf.Max(0f, initialDelay));

        // ターゲット取得（タグ "Player"）
        GameObject go = GameObject.FindWithTag("Player");
        if (go != null) target = go.transform;

        isHoming = true;
        float endTime = Time.time + Mathf.Max(0f, homingDuration);

        // homingDuration 継続して回転と移動を行う
        while (Time.time < endTime)
        {
            // ターゲットが存在すれば向きを合わせる（滑らかに回転）
            if (target != null)
            {
                Vector3 toTarget = target.position - transform.position;
                toTarget.z = 0f;
                if (toTarget.sqrMagnitude > 1e-6f)
                {
                    float targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                    Quaternion targetRot = Quaternion.Euler(0f, 0f, targetAngle);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                }
            }

            // 前進（回転によって向きが変わるため、transform.right を使う）
            transform.position += transform.right * speed * Time.deltaTime;

            yield return null;
        }

        // 追尾終了 ? 現在の向きを保持して以降は Update の直進処理に任せる
        isHoming = false;
        homingCoroutine = null;
    }

    void OnDisable()
    {
        if (homingCoroutine != null)
            StopCoroutine(homingCoroutine);
    }
}