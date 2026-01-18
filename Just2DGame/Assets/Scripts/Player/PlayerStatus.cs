using UnityEngine;
using System.Collections;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] float invincibilityDuration = 1.0f; // ñ≥ìGéûä‘ÅiïbÅj

    public int hp = 100;
    public bool Invincible = false;

    Coroutine invincibillty;

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
        invincibillty = StartCoroutine(InvincibilityCoroutine(invincibilityDuration));

        hp -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP = {hp}");
        if (hp <= 0) Die();
    }

    void Die()
    {
        // ä»à’ìIÇ»èàóùó·
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
