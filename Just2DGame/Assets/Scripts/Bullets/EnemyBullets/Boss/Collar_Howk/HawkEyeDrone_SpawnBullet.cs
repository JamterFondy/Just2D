using UnityEngine;

public class HawkEyeDrone_SpawnBullet : MonoBehaviour
{
    [SerializeField] GameObject bullet;

    GameObject player;

    [SerializeField] float bulletSpawn_Span = 3f; // ’e‚Ì”­ËŠÔŠui•bj
    [SerializeField] float adugstBullet_Rotate = -90f; // ’e‚Ì‰ñ“]•â³Šp“x

    void Start()
    {
        InvokeRepeating("SpawnBullet", 0f, bulletSpawn_Span);
    }

    void SpawnBullet()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return; // ƒvƒŒƒCƒ„[‚ª‘¶İ‚µ‚È‚¢ê‡‚Í‰½‚à‚µ‚È‚¢

        Vector2 playerPos = player.transform.position;
        Vector2 direction = (playerPos - (Vector2)transform.position).normalized;

        float playerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, playerAngle + adugstBullet_Rotate);

        GameObject bulletPrefab = Instantiate(bullet, transform.position, rotation);

        
    }
}
