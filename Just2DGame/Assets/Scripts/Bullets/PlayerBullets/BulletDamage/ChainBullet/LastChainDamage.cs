using UnityEngine;

public class LastChainDamage : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    public int atk;
    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = FindObjectOfType<PlayerStatus>();

        atk = playerStatus.atk;

        damage = (int)(atk * 0.8f);
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
