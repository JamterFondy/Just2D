using UnityEngine;
using System.Collections;


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

public class FrainCycleManager : MonoBehaviour
{
    [SerializeField] FrainMovement frainMovement;

    public FrainState currentState;

    int cycleChangedCount = 0; // サイクルの変化回数をカウント
    int cycleInterval = 1; // サイクルの変化間隔（秒）
    int commandPattern = 0;

    bool isMoveCompleted = false;

    void Start()
    {
        StartCoroutine(CycleManagement());
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
        if (commandPattern == 1)
        {
            int randomPosX = Random.Range(2, 9);

            StartCoroutine(frainMovement.MoveVerticle(randomPosX, 3));
            isMoveCompleted = false;
        }
        else if (commandPattern == 2)
        {
            int randomPosX = Random.Range(2, 9);

            StartCoroutine(frainMovement.MoveVerticle(randomPosX, -3));
            isMoveCompleted = false;
        }
        else if (commandPattern == 3)
        {
            StartCoroutine(frainMovement.MoveTriangle(4.5f, 0, 2, -3, 2, 3, 8f, 0));
            isMoveCompleted = false;
        }


        yield return new WaitUntil(() => isMoveCompleted);
        yield return new WaitForSeconds(1f);


        commandPattern = Random.Range(4, 12); // 攻撃パターンをランダムに選択

        if (commandPattern <= 8)
        {
            StartCoroutine(NormalAttackCommand(commandPattern));
        }
        else
        {
            StartCoroutine(SkillAttackCommand(commandPattern));
        }

        Debug.Log("攻撃パターン " + commandPattern);

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

        Debug.Log("移動パターン " + commandPattern);

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

        Debug.Log("移動パターン " + commandPattern);

        yield break;
    }


    IEnumerator ULTCommand()
    {
        //右画面中央に移動。カットインを挟んだのち自身の周囲に大きな円形の炎の弾を5つ生成してプレイヤーの位置に流す。
        //大型の炎からは小型の炎が複数生成されてプレイヤーに向かって流れる。

        yield break;
    }


}
