using UnityEngine;
using System.Collections;

public class EnemyStatus : MonoBehaviour
{
    public int hp = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamage(int amount)
    {
        hp -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP = {hp}");
        if (hp <= 0) Die();
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
