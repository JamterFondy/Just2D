using UnityEngine;
using System.Collections;

public class ZakoSpeedAll : MonoBehaviour
{
    [SerializeField] ZakoSpeedSO zakoSpeedSO;
    [SerializeField] GameObject player;

    Rigidbody2D rb;

    public float hp;
    public float atk;
    public float def;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = GameObject.FindGameObjectWithTag("Player");

        hp = zakoSpeedSO.hp;
        atk = zakoSpeedSO.atk;
        def = zakoSpeedSO.def;
        speed = zakoSpeedSO.speed;

        StartCoroutine(Movement());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public IEnumerator Movement() //この動きの処理
    {
        // 1. スポーンしてから1秒間は左に移動
        rb.linearVelocity = Vector2.left * speed * 0.3f;
        yield return new WaitForSeconds(1);

        // 2. 停止
        rb.linearVelocity = Vector2.zero;

        // 3. プレイヤー追従（2秒間）
        float followDuration = 2f;
        float elapsed = 0f;
        float followSpeed = speed;

        while (elapsed < followDuration)
        {
            if (player != null)
            {
                Vector2 myPos = transform.position;
                Vector2 playerPos = player.transform.position;
                Vector2 direction = (playerPos - myPos).normalized;
                float distance = Vector2.Distance(myPos, playerPos);

                // 距離が遠いほど速く、近いほど遅く
                float minSpeed = speed * 0.3f;
                float maxSpeed = speed * 1.0f;
                followSpeed = Mathf.Lerp(minSpeed, maxSpeed, Mathf.Clamp01(distance / 10f));

                rb.linearVelocity = direction * followSpeed;

                // プレイヤーの方向をx軸の負の方向（左）に合わせる
                // 進行方向が左（-x）になるように回転
                if (direction != Vector2.zero)
                {
                    // x軸負方向（左）をforwardにするため、directionの逆向き
                    float angle = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 4. 追従終了後、現在向いている方向（x軸の負の方向）に現在の速度で進む
        Vector2 currentVelocity = rb.linearVelocity;
        Vector2 moveDir = currentVelocity.normalized;
        if (moveDir == Vector2.zero)
        {
            moveDir = Vector2.left;
        }
        rb.linearVelocity = moveDir * followSpeed;

        // 進行方向に自身の向きを合わせる
        if (moveDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(-moveDir.y, -moveDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        yield return new WaitForSeconds(3);
        Destroy(gameObject);

        yield return null;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();
            if (playerStatus != null)
            {
                playerStatus.ApplyDamage((int)atk);
            }
        }
        else if (collision.CompareTag("Bullet"))
        {
            int damage = 0;
            // ChainDamageなど、弾の種類ごとにダメージを取得
            var chainDamage = collision.GetComponent<ChainDamage>();
            if (chainDamage != null)
            {
                damage = chainDamage.GetDamage();
            }
            // 他の弾種も同様にGetDamage()を持たせておけば拡張可能

            if (damage > 0)
            {
                ApplyDamage(damage);
            }
        }
    }

    public void ApplyDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
