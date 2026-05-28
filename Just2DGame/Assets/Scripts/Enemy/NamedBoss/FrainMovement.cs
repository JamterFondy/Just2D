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

    int cycleChangedCount = 0; // サイクルの変化回数をカウント
    int cycleInterval = 1; // サイクルの変化間隔（秒）

    int hp = 10000; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。
　　int atk = 25; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。
    int speed = 20; // EnemyStatusを全ての敵に適応できる形にしたら消すこと。

    void Start()
    {
        StartCoroutine(CycleManagement());
    }

    IEnumerator CycleManagement()
    {     
        int commandPattern = Random.Range(1, 12); // 動きと行動のパターンをランダムに選択
                                                  // 1から3は移動パターン。攻撃なし。4から8は通常攻撃。9から11はスキル攻撃。12はカットインを挟んでからウルト攻撃。

        // 1は八の字を描くように移動。攻撃はなし。
        // 2は右画面中央で上下に移動。攻撃はなし。
        // 3は三角形を描くように高速移動。攻撃はなし。

        // 4は下から上に移動。移動中に自身の位置に炎を生成して左側に流す。
        // 5は上から下に移動。移動中に自身の位置に炎を生成して左側に流す。
        // 6は右画面中央に移動。左水平方向に炎を生成して上下に流す。
        // 7は右画面上側に移動。左下に向けて炎の弾を生成して斜めに流す。
        // 8は右画面下側に移動。左上に向けて炎の弾を生成して斜めに流す。

        // 9は右画面中央に移動。左画面のランダムな位置３個所に危険信号を生成した後、その位置に円形に炎の弾を生成して流す。
        // 10は右画面中央に移動。左画面のランダムな位置5個所に危険信号を順次生成した後、その位置に円形に炎の弾を生成して流す。
        // 11は右画面中央に移動。画面水平中央に太い危険信号を生成した後、その軌道をなぞるように高速移動。通った軌道に沿って炎の弾を生成して斜めに流す。

        // 12は右画面中央に移動。カットインを挟んだのち自身の周囲に大きな円形の炎の弾を5つ生成してプレイヤーの位置に流す。大型の炎からは小型の炎が複数生成されてプレイヤーに向かって流れる。

        StartCoroutine(RandomCommand(commandPattern));

        yield return null;
    }

    IEnumerator RandomCommand(int commandPattern)
    {
        if(commandPattern == 1)
        {
            // Move pattern 1 logic
        }
        else if(commandPattern == 2)
        {
            // Move pattern 2 logic
        }
        else if(commandPattern == 3)
        {
            // Move pattern 3 logic
        }

        yield return null;
    }

}
