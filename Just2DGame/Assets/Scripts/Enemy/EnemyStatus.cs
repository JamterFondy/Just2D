using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] CollarBoss collarBoss;
    [SerializeField] LoadingManager loadingManager;
    BossEnemy bossEnemy;

    public int hp;
    public int atk;
    public int def;
    public int speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loadingManager = FindAnyObjectByType<LoadingManager>();
        bossEnemy = gameObject.GetComponent<BossEnemy>();

        hp = (int)collarBoss.runtimeHP;
        atk = (int)collarBoss.runtimeATK;
        def = (int)collarBoss.runtimeDEF;
        speed = (int)collarBoss.runtimeSPEED;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();
          
        if (collision.CompareTag("Bullet"))
        {
            int damage = 0;

            // 弾側で計算したダメージを取得するためのコンポーネントを取得
            // Chara1
            var chara1NormalDamage = collision.GetComponent<NormalDamage>();
            var chainDamage = collision.GetComponent<ChainDamage>();
            var lastChainDamage = collision.GetComponent<LastChainDamage>();

            // Chara2
            var chara2NormalDamage = collision.GetComponent<Chara2NormalDamage>();
            var fishDamage = collision.GetComponent<FishDamage>();

            // 弾の種類に応じたダメージの取得

            if (chara1NormalDamage != null) // ここからChara1の弾のダメージ判定
            {
                damage = chara1NormalDamage.GetDamage();
            }
            else if (chainDamage != null)
            {
                damage = chainDamage.GetDamage();
            }
            else if (lastChainDamage != null)
            {
                damage = lastChainDamage.GetDamage();
            }
            else if (chara2NormalDamage != null) // ここからChara2の弾のダメージ判定
            {
                damage = chara2NormalDamage.GetDamage();
            }
            else if (fishDamage != null)
            {
                damage = fishDamage.GetDamage();
            }


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
            // ボスエネミーであるなら個別の処理を行う
            if (bossEnemy == null)
            {
                Die();
            }
            else
            {
                bossEnemy.BossDied();
            }
        }
    }

    void Die()
    {
        
        Debug.Log($"{gameObject.name} died.");

        Destroy(gameObject);
    }

}
