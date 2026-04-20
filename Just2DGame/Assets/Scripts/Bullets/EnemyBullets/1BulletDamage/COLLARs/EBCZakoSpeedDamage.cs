using UnityEngine;

public class EBCZakoSpeedDamage : MonoBehaviour
{
    [SerializeField] ZakoSpeedAll zakoSpeedAll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        zakoSpeedAll = GameObject.Find("COLLARZakoSpeed").GetComponent<ZakoSpeedAll>();
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
                playerstatus.ApplyDamage((int)(zakoSpeedAll.atk)); // Example damage value
            }
        }
    }
}
