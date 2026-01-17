using UnityEngine;

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
        // ŠÈˆÕ“I‚Èˆ——á
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}
