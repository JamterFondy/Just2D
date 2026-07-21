using UnityEngine;

public class Hawk_SkillCycle : MonoBehaviour
{
    EnemyStatus enemyStatus;
    Spawn_HawkEyeDrone spawn_HawkEyeDrone;

    bool hasSpawned = false; 

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    
    void Update()
    {
        if (enemyStatus.hp <= enemyStatus.maxHp * 0.5f && !hasSpawned)
        {
            spawn_HawkEyeDrone = GetComponent<Spawn_HawkEyeDrone>();

            spawn_HawkEyeDrone.SpawnDrone_Activate();
            hasSpawned = true;
        }
    }
}
