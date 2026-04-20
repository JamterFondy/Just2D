using UnityEngine;

public class ELineBullet : MonoBehaviour
{
    [SerializeField] float speed = 10f; // 左方向への移動速度（Inspectorで変更可）

    void Start()
    {

    }

    void Update()
    {
        // "向いている方向" を前方ベクトルとして transform.right を使用
        Vector3 forward = transform.right;
        // 前方ベクトルに対する左方向（2D平面での垂直ベクトル）
        Vector3 left = new Vector3(-forward.y, forward.x, 0f);

        transform.position += left.normalized * speed * Time.deltaTime;
    }
}
