using UnityEngine;

public class NormalBullet : MonoBehaviour
{
    [SerializeField] float speedPerFrame = 0.5f; // 毎フレームの移動量（ユニット/フレーム）

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position -= transform.up * speedPerFrame;
    }
}
