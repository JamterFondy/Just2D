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

            //弾側で計算したダメージを取得するためのコンポーネントを取得
            //Chara1
            var chara1NormalDamage = collision.GetComponent<NormalDamage>();
            var chainDamage = collision.GetComponent<ChainDamage>();
            var lastChainDamage = collision.GetComponent<LastChainDamage>();

            //Chara2
            var chara2NormalDamage = collision.GetComponent<Chara2NormalDamage>();
            var fishDamage = collision.GetComponent<FishDamage>();

            //弾の種類に応じたダメージの取得

            if (chara1NormalDamage != null)//ここからChara1の弾のダメージ判定
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
            else if (chara2NormalDamage != null) //ここからChara2の弾のダメージ判定
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
