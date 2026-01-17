using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float speed = 6f;
    Camera cam;

    [SerializeField] GameObject normalBullet; // 通常弾

    bool PlayerMoving = false; // プレイヤーが移動中かどうかのフラグ

    Vector3 pos;
   

    void Start()
    {
        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("Main Camera not found. Movement bounds will not be applied.");

        StartCoroutine(SpawnNormalBullet());
    }

    void Update()
    {

        pos = transform.position;

        // 入力（W/A/S/D）
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.W)) input.y += 1f;
        if (Input.GetKey(KeyCode.S)) input.y -= 1f;
        if (Input.GetKey(KeyCode.D)) input.x += 1f;
        if (Input.GetKey(KeyCode.A)) input.x -= 1f;
        if (input.sqrMagnitude > 1f) input.Normalize();

        if(input.x != 0 && input.y != 0)
        {
            PlayerMoving = true;
        }
        else
        {
            PlayerMoving = false;
        }

        // 移動
        Vector3 delta = new Vector3(input.x, input.y, 0f) * speed * Time.deltaTime;
        transform.position += delta;

        // カメラの左半分矩形内にクランプ（カメラがある場合）
        if (cam != null)
        {

            // カメラとプレイヤーのZ差を指定して正しいワールド座標を得る
            float distance = transform.position.z - cam.transform.position.z;

            // ビューポートからワールド座標を計算
            Vector3 leftBottom = cam.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
            Vector3 rightMiddle = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            Vector3 top = cam.ViewportToWorldPoint(new Vector3(0f, 1f, distance));

            float minX = leftBottom.x;
            float maxX = rightMiddle.x;    // カメラ幅の中央（左半分の右端）
            float minY = leftBottom.y;
            float maxY = top.y;

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            transform.position = pos;
        }

    }


    IEnumerator SpawnNormalBullet()
    {
        Quaternion angler = Quaternion.Euler(0f, 0f, 90f);

        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            Instantiate(normalBullet, new Vector3(pos.x - 2f, pos.y, pos.z), angler);
            Instantiate(normalBullet, new Vector3(pos.x - 2f, pos.y + 0.2f, pos.z), angler);
            Instantiate(normalBullet, new Vector3(pos.x - 2f, pos.y - 0.2f, pos.z), angler);

        }
           
    }

    
}
