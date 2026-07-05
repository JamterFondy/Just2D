using UnityEngine;
using System.Collections;

public enum EnemyState
{
    // 一般種類

    Idle,
    Move,
    Attack,
    Damaged,
    Die
}

public enum Affected
{
    // 外部からの状態異常

    None, //状態なしor状態回復
    Restricted, // 移動制限状態
    Resonance, // 共鳴状態
    Burn // 燃焼状態
}

public class EnemyStatus : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] EnemyGeneralStatus enemyGeneralStatus;
    [SerializeField] LoadingManager loadingManager;
    BossEnemy bossEnemy;

    public int hp;
    public int atk;
    public int def;
    public int speed;

    public EnemyState currentState;
    public Affected currentAffected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        loadingManager = FindAnyObjectByType<LoadingManager>();
        bossEnemy = this.gameObject.GetComponent<BossEnemy>();

        hp = enemyGeneralStatus.hp;
        atk = enemyGeneralStatus.atk;
        def = enemyGeneralStatus.def;
        speed = (int)enemyGeneralStatus.speed;

        currentState = EnemyState.Idle;
        currentAffected = Affected.None;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatus playerStatus = collision.GetComponent<PlayerStatus>();
          
        if (collision.CompareTag("Bullet"))
        {
            int damage = 0;

            // 弾側で計算したダメージを取得するためのコンポーネントを取得
            // Chara1
            var chara1NormalDamage = collision.GetComponent<NormalDamage>(); // ★ 当たった弾にコンポーネントがあるかどうか分かるなら、弾側でダメージ計算だけして、直接ApplyDamageを呼び出せばいいのでは？
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

                StartCoroutine(GetAffected(Affected.Restricted, 2f)); // ToDo 一時的に値を設定しているだけ。本来は外部(弾側)からこの関数を呼び出すか、秒数を外部から受け渡す形にするべき。
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

    public IEnumerator GetAffected(Affected AffectType, float AffectTime)
    {
        currentAffected = AffectType;

        yield return new WaitForSeconds(AffectTime);

        currentAffected = Affected.None;

        yield break;
    }

    void Die()
    {
        
        Debug.Log($"{gameObject.name} died.");

        Destroy(gameObject);
    }

}
