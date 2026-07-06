using System.Collections;
using UnityEngine;

public enum PlayerULTStatus
{
    Using, // 使用中
    Charging, // CD中
    FullCharge // 使用可能状態
}

public class PlayerULTActivate : MonoBehaviour
{
    PlayerULTStatus currentULTStatus;

    PlayerStatus playerStatus;

    [SerializeField] Sprite[] cutIn_sprite;


    [SerializeField] int ULTBallNum; // ウルトの発動可能回数（ボムの数）
    int charaID;

    float ULT_CDTime;

    bool isULT_CutInFinished;

    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();

        charaID = playerStatus.characterID;
        ULT_CDTime = playerStatus.ULTCD;

    }

    void Update()
    {
        // ウルトの起動
        if(currentULTStatus == PlayerULTStatus.FullCharge && Input.GetKeyDown(KeyCode.R) && ULTBallNum > 0)
        {
            StartCoroutine(ULTActivated(charaID, ULT_CDTime));
        }
    }

    IEnumerator ULTActivated(int charaID, float CDTime)
    {
        currentULTStatus = PlayerULTStatus.Using;
        playerStatus.currentControlState = PlayerControlState.ULT;

        ULTBallNum --;
        isULT_CutInFinished = false;

        StartCoroutine(ULT_CutIn(charaID));

        yield return new WaitUntil(() => isULT_CutInFinished == true);

        // ここでULTの弾を生成する処理を呼び出す。生成用の別のクラスを作っておけ。
        // 生成用のスクリプトには他の弾と同様にGameObject[]の配列を用意して、charaIDを参考に生成する弾と生成パターンを決定するようにする。
        // 生成する弾のパターンに関してはリスト内の弾にULT用のコンポーネントを付与して、その処理を発火させる形ができないかを検討する。


        playerStatus.currentControlState = PlayerControlState.None; // 敵味方、弾の移動を許可する。

        // 生成が完了したかどうかを弾側からboolで受けて、完了してから以下の処理を行う形にしても良いかも。

        currentULTStatus = PlayerULTStatus.Charging; // クールタイムを挟む。

        yield break;
    }

    IEnumerator ULT_CutIn(int charaID)
    {
        if(cutIn_sprite[charaID - 1] != null && cutIn_sprite.Length > 0)
        {
            Sprite currentCutIn = cutIn_sprite[charaID - 1];
        }

        // ULTのカットインを流すUIをアタッチして参照。
        // 流すアニメーションはこちらで指定（現在はSpriteになっている部分）するか、カットインUI側にcharaIDを引き渡して流させるか。
        // アニメーションの長さを統一できない場合はカットインUI側からカットインの終了判定を返り値として受け取る形にするべき。


        yield return new WaitForSeconds(2); // 演出時間を二秒として、演出終了するまでは弾の生成を行わないようにする。あくまで仮置き。

        isULT_CutInFinished = true;

    }

}
