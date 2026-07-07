using UnityEngine;

public class EBCZakoSpeedDamage : MonoBehaviour
{
    public int bulletDamage;

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStatus playerstatus = other.GetComponent<PlayerStatus>();

        if (other.gameObject.tag == "Player")
        {
            playerstatus = other.GetComponent<PlayerStatus>();

            if (playerstatus != null && !playerstatus.Invincible)
            {
                // Apply damage to the object
                playerstatus.ApplyDamage((int)(bulletDamage)); // Example damage value
            }
        }
    }
}
