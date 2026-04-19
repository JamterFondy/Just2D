using UnityEngine;

public class SimpleBulletMover : MonoBehaviour
{
    float speed = 5f;
    Vector3 direction = Vector3.right;

    public void Initialize(float s, Vector3 dir)
    {
        speed = s;
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }
}
