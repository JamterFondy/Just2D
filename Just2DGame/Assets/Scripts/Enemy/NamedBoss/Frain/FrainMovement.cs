using System.Collections;
using UnityEngine;

public class FrainMovement : MonoBehaviour
{

    Rigidbody2D rb;

    int hp = 10000; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。
　　int atk = 25; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。
    int speed = 5; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。


    Vector2 currentPos;
    Vector2 targetPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    public IEnumerator MoveVerticle(int x, int y)
    {
        Vector2 currentPos = rb.position;
        Vector2 targetPos = new Vector2(x, y);
        Vector2 dir = (targetPos - currentPos).normalized;

        // 移動
        rb.linearVelocity = dir * speed;

        // ② 0.1未満ならスナップ
        if (Vector2.Distance(currentPos, targetPos) < 0.1f)
        {
            rb.position = targetPos;
            rb.linearVelocity = Vector2.zero;
        }

        yield break;
    }

    public IEnumerator MoveTriangle(float centerX, float centerY, float firstX, float firstY, float secondX, float secondY, float thirdX, float thirdY)
    {


        yield break;
    }




}
