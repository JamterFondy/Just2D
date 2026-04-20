using UnityEngine;

public class ELineBulletDamage : MonoBehaviour
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
        PlayerStatus playerstatus = other.GetComponent<PlayerStatus>();

        if (other.gameObject.tag == "Player")
        {
            playerstatus = other.GetComponent<PlayerStatus>();

            if (playerstatus != null && !playerstatus.Invincible)
            {
                // Apply damage to the object
                playerstatus.ApplyDamage(10); // Example damage value
            }
        }
    }
}
