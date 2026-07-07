using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" || collision.gameObject.tag == "EBullet")
        {
            Destroy(collision.gameObject);
        }
    }

}
