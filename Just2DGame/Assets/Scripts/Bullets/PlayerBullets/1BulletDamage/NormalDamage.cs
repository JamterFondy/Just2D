using UnityEngine;

public class NormalDamage : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    public int atk;
    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();

        atk = playerStatus.atk;

        if(atk >= 10)
        {
            damage = (int)(atk * 0.1f);
        }
        else
        {
            damage = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetDamage()
    {
        return damage;
    }
}
