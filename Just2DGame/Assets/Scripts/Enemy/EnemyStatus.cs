using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] LoadingManager loadingManager;

    public int hp = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loadingManager = FindObjectOfType<LoadingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();
          
        if (collision.CompareTag("Bullet"))
        {
            int damage = 0;
            // ChainDamageなど、弾の種類ごとにダメージを取得
            var normalDamage = collision.GetComponent<NormalDamage>();
            var chainDamage = collision.GetComponent<ChainDamage>();
            var lastChainDamage = collision.GetComponent<LastChainDamage>();

            if (normalDamage != null)
            {
                damage = normalDamage.GetDamage();
            }
            else if (chainDamage != null)
            {
                damage = chainDamage.GetDamage();
            }
            else if (lastChainDamage != null)
            {
                damage = lastChainDamage.GetDamage();
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

            Die();
        }
    }

    void Die()
    {
        // 簡易的な処理例
        Debug.Log($"{gameObject.name} died.");

        // シーン遷移処理をコルーチンで開始（2秒待って MapScene に移動）
        BattleFinish.Instance.MoveToMapAfterDelay();

        Destroy(gameObject);
    }

}
