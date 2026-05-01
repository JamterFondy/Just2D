using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    CharaInfoServer charaInfoServer;//IDサーバーオブジェクト
    BattleFinish battleFinish;//バトル終了オブジェクト

    public float invincibilityDuration = 1.0f; // 無敵時間（秒）

    public int characterID;//キャラクターID番号
    public int hp;// プレイヤーのHP
    public int atk;// プレイヤーの攻撃力


    public bool Invincible = false;// 無敵状態かどうか


    public float leftCrickCT;//スキル弾のクールタイム
    public bool LeftCrickCTBool;//スキル弾のクールタイム中かどうか

    Coroutine invincibillty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        charaInfoServer = GameObject.Find("CharaInfoServer").GetComponent<CharaInfoServer>();
        battleFinish = GameObject.Find("BattleFinish").GetComponent<BattleFinish>();

        characterID = charaInfoServer.ID;
        hp = charaInfoServer.HP;
        atk = charaInfoServer.ATK;
    }

    

    public void ApplyDamage(int amount)
    {
        invincibillty = StartCoroutine(InvincibilityCoroutine(invincibilityDuration));

        hp -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP = {hp}");
        if (hp <= 0) Die();
    }

    void Die()
    {
        // 簡易的な処理例
        Debug.Log($"You died.");

        battleFinish.MoveToMapAfterDelay();

        Destroy(gameObject);
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        Invincible = true;
        yield return new WaitForSeconds(duration);
        Invincible = false;
    }
}
