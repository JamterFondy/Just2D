using UnityEngine;

public class Chara2NormalDamage : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    public int atk;
    public int damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();

        atk = playerStatus.atk;

        if(atk * 0.05f <= 1)
        {
            damage = 1;
        }
        else
        {
            damage = (int)(atk * 0.05f);
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
