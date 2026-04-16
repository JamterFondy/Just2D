using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] IDServer idServer;//IDサーバーオブジェクト
    [SerializeField] float invincibilityDuration = 1.0f; // 無敵時間（秒）

    public int CharacterID;//キャラクターID番号

    public int hp = 100;// プレイヤーのHP
    public bool Invincible = false;// 無敵状態かどうか


    public float leftCrickCT;//スキル弾のクールタイム
    public bool LeftCrickCTBool;//スキル弾のクールタイム中かどうか

    Coroutine invincibillty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idServer = GameObject.Find("IDServer").GetComponent<IDServer>();

        CharacterID = idServer.ID;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Destroy(gameObject);
    }

    IEnumerator InvincibilityCoroutine(float duration)
    {
        Invincible = true;
        yield return new WaitForSeconds(duration);
        Invincible = false;
    }
}
