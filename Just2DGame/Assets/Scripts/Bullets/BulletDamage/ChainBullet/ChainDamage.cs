using UnityEngine;

public class ChainDamage : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // Check if the object has a Damageable component
            EnemyStatus enemystatus = other.GetComponent<EnemyStatus>();
            if (enemystatus != null)
            {
                // Apply damage to the object
                enemystatus.ApplyDamage(10); // Example damage value
            }
        }
    }
}
