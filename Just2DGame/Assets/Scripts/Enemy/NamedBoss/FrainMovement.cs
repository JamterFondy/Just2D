using System.Collections;
using UnityEngine;

public enum FrainState
{
    Idle,
    Moving,
    Attacking,
    Talking,
    Skill1,
    Skill2,
    Skill3,
    ULT,
    Dying
}

public class FrainMovement : MonoBehaviour
{
    public FrainState currentState;

    Rigidbody2D rb;

    int cycleChangedCount = 0; // サイクルの変化回数をカウント
    int cycleInterval = 1; // サイクルの変化間隔（秒）

    int hp = 10000; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。
　　int atk = 25; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。
    int speed = 5; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。

    int commandPattern = 0;

    Vector2 currentPos;
    Vector2 targetPos;
    bool isMoving12 = false;
    bool isMoving3 = false;
    bool isMoveCompleted = false;

    bool isMove2FirstPos = false;
    bool isMove2SecondPos = false;
    bool isMove2ThirdPos = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPos = rb.position;

        StartCoroutine(CycleManagement());
    }


    void FixedUpdate()
    {
        if (!isMoving12 && !isMoving3) return;

        if(commandPattern == 1 || commandPattern == 2)
        {
            currentPos = rb.position;
            Vector2 dir = (targetPos - currentPos).normalized;

            // 移動
            rb.linearVelocity = dir * speed;

            // ② 0.1未満ならスナップ
            if (Vector2.Distance(currentPos, targetPos) < 0.1f)
            {
                rb.position = targetPos;
                rb.linearVelocity = Vector2.zero;

                isMoving12 = false;
                isMoveCompleted = true;
            }
        }


        if(commandPattern == 3)
        {

            if(!isMove2FirstPos && !isMove2SecondPos && !isMove2ThirdPos)
            {
                currentPos = rb.position;
                Vector2 dir = (targetPos - currentPos).normalized;

                rb.linearVelocity = dir * speed * 0.5f;

                if (Vector2.Distance(currentPos, targetPos) < 0.1f)
                {
                    rb.position = targetPos;
                    isMove2FirstPos = true;
                    Debug.Log("最初の地点に移動");
                }
            }
            else if(isMove2FirstPos)
            {
                targetPos = new Vector2(2, -3);

                currentPos = rb.position;
                Vector2 dir = (targetPos - currentPos).normalized;

                rb.linearVelocity = dir * speed * 1.2f;

                if (Vector2.Distance(currentPos, targetPos) < 0.1f)
                {
                    rb.position = targetPos;
                    isMove2SecondPos = true;
                    isMove2FirstPos = false;
                    Debug.Log("1番目の地点に移動");
                }
            }
            else if (isMove2SecondPos)
            {
                targetPos = new Vector2(2, 3);

                currentPos = rb.position;
                Vector2 dir = (targetPos - currentPos).normalized;

                rb.linearVelocity = dir * speed * 1.2f;

                if (Vector2.Distance(currentPos, targetPos) < 0.1f)
                {
                    rb.position = targetPos;
                    isMove2ThirdPos = true;
                    isMove2SecondPos = false;
                    Debug.Log("2番目の地点に移動");
                }
            }
            else if (isMove2ThirdPos)
            {
                targetPos = new Vector2(8, 0);

                currentPos = rb.position;
                Vector2 dir = (targetPos - currentPos).normalized;

                rb.linearVelocity = dir * speed * 1.2f;

                if (Vector2.Distance(currentPos, targetPos) < 0.1f)
                {
                    rb.position = targetPos;
                    isMoving3 = false;
                    isMoveCompleted = true;
                    isMove2ThirdPos = false;
                    Debug.Log("最期の地点に移動");

                }
            }


        }
        
    }


    IEnumerator CycleManagement()
    {     
        commandPattern = Random.Range(1, 4); // 動きと行動のパターンをランダムに選択
                                                  // 1から3は移動パターン。攻撃なし。4から8は通常攻撃。9から11はスキル攻撃。12はカットインを挟んでからウルト攻撃。

        // 1は上に移動。攻撃はなし。
        // 2は下に移動。攻撃はなし。
        // 3は三角形を描くように高速移動。攻撃はなし。
        StartCoroutine(MoveCommand(commandPattern));


        // 4は下から上に移動。移動中に自身の位置に炎を生成して左側に流す。
        // 5は上から下に移動。移動中に自身の位置に炎を生成して左側に流す。
        // 6は右画面中央に移動。左水平方向に炎を生成して上下に流す。
        // 7は右画面上側に移動。左下に向けて炎の弾を生成して斜めに流す。
        // 8は右画面下側に移動。左上に向けて炎の弾を生成して斜めに流す。
        StartCoroutine(NormalAttackCommand(commandPattern));


        // 9は右画面中央に移動。左画面のランダムな位置３個所に危険信号を生成した後、その位置に円形に炎の弾を生成して流す。
        // 10は右画面中央に移動。左画面のランダムな位置5個所に危険信号を順次生成した後、その位置に円形に炎の弾を生成して流す。
        // 11は右画面中央に移動。画面水平中央に太い危険信号を生成した後、その軌道をなぞるように高速移動。通った軌道に沿って炎の弾を生成して斜めに流す。
        StartCoroutine(SkillAttackCommand(commandPattern));


        yield break;
    }


    IEnumerator MoveCommand(int commandPattern)
    {
        isMoveCompleted = true;

        if(commandPattern == 1 && isMoveCompleted)
        {
            int randomPosX = Random.Range(2, 9);
            targetPos = new Vector2(randomPosX, 3);

            isMoveCompleted = false;
            isMoving12 = true;

        }
        else if(commandPattern == 2 && isMoveCompleted)
        {
            int randomPosX = Random.Range(2, 9);
            targetPos = new Vector2(randomPosX, -3);

            isMoveCompleted = false;
            isMoving12 = true;
        }
        else if(commandPattern == 3 && isMoveCompleted)
        {
            targetPos = new Vector2(4.5f, 0);

            isMoveCompleted = false;
            isMoving3 = true;
        }


        yield return new WaitUntil(() => isMoveCompleted);
        yield return new WaitForSeconds(1f);


        commandPattern = Random.Range(4, 12); // 攻撃パターンをランダムに選択

        if(commandPattern <= 8)
        {
            StartCoroutine(NormalAttackCommand(commandPattern));
        }
        else
        {
            StartCoroutine(SkillAttackCommand(commandPattern));
        }

        yield break;
    }


    IEnumerator NormalAttackCommand(int commandPattern)
    {
        if (commandPattern == 4)
        {
            // Normal attack pattern 1 logic
        }
        else if (commandPattern == 5)
        {
            // Normal attack pattern 2 logic
        }
        else if (commandPattern == 6)
        {
            // Normal attack pattern 3 logic
        }
        else if (commandPattern == 7)
        {
            // Normal attack pattern 4 logic
        }
        else if (commandPattern == 8)
        {
            // Normal attack pattern 5 logic
        }


        commandPattern = Random.Range(1, 4); // 次の移動パターンをランダムに選択
        StartCoroutine(MoveCommand(commandPattern));

        yield break;
    }


    IEnumerator SkillAttackCommand(int commandPattern)
    {
        if (commandPattern == 9)
        {
            // Skill attack pattern 1 logic
        }
        else if (commandPattern == 10)
        {
            // Skill attack pattern 2 logic
        }
        else if (commandPattern == 11)
        {
            // Skill attack pattern 3 logic
        }


        commandPattern = Random.Range(1, 4); // 次の移動パターンをランダムに選択
        StartCoroutine(MoveCommand(commandPattern));

        yield break;
    }


    IEnumerator ULTCommand()
    {
        //右画面中央に移動。カットインを挟んだのち自身の周囲に大きな円形の炎の弾を5つ生成してプレイヤーの位置に流す。
        //大型の炎からは小型の炎が複数生成されてプレイヤーに向かって流れる。

        yield break;
    }

}
