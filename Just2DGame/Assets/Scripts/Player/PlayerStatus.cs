using UnityEngine;
using System.Collections;
using System;


public enum PlayerControlState
{
    None,
    BattleStart,
    BattleFinished,
    ULT // ULT状態の時は敵も弾も全て移動不可にする
}

public class PlayerStatus : MonoBehaviour
{
    CharaInfoServer charaInfoServer;//IDサーバーオブジェクト
    BattleFinish battleFinish;//バトル終了オブジェクト

    [SerializeField] GameObject[] playerBulletSets; // キャラクターの弾セット

    public float invincibilityDuration = 1.0f; // 無敵時間（秒）

    public int characterID;//キャラクターID番号
    public int hp;// プレイヤーのHP
    public int atk;// プレイヤーの攻撃力
    public int def;// プレイヤーの防御力
    public int spd;// プレイヤーの移動速度
    public int skillLv;// プレイヤーのスキルレベル
    public int skillCD;// プレイヤーのスキルクールダウン
    public int ULTLv;// プレイヤーのウルトレベル
    public int ULTCD;// プレイヤーのウルトクールダウン


    public bool Invincible = false;// 無敵状態かどうか


    public float leftCrickCT;//スキル弾のクールタイム
    public bool LeftCrickCTBool;//スキル弾のクールタイム中かどうか

    Coroutine invincibillty;


    public event Action<PlayerControlState> PlayerControlStateChanged;

    PlayerControlState _currentControlState;
    public PlayerControlState currentControlState
    {
        get => _currentControlState;
        set
        {
            if (_currentControlState != value)
            {
                if (_currentControlState == value) return;
                _currentControlState = value;
                PlayerControlStateChanged?.Invoke(_currentControlState);
            }
        }
    }

    void Awake()
    {
        charaInfoServer = GameObject.Find("CharaInfoServer").GetComponent<CharaInfoServer>();
        battleFinish = GameObject.Find("BattleFinish").GetComponent<BattleFinish>();

        characterID = charaInfoServer.ID;
        hp = charaInfoServer.HP;
        atk = charaInfoServer.ATK;


        // 今回選択したキャラの弾セットのみを有効化する

        foreach (var bulletSetObj in playerBulletSets)
        {
            bulletSetObj.SetActive(false);
        }

        playerBulletSets[characterID - 1].SetActive(true);
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

        battleFinish.StartCoroutine(battleFinish.PlayerDied());

        Destroy(gameObject);
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        Invincible = true;
        yield return new WaitForSeconds(duration);
        Invincible = false;
    }
}
