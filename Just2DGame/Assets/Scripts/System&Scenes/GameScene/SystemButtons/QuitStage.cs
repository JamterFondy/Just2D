using UnityEngine;

public class QuitStage : MonoBehaviour
{
    BattleFinish battleFinish;
    BattleESC battleESC;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnClick()
    {
        battleFinish = FindAnyObjectByType<BattleFinish>();
        battleESC = FindAnyObjectByType<BattleESC>();

        battleFinish.QuitStage(); //ToDo⇒◆Check◆ ESCが有効だとコルーチンによるシーン切り替えができない。すぐにシーン移動する形に切り替えるか、またはBattleESCの参照を取ってIsPosedを強制的に解除（false）にする。
        battleESC.QuitStage();
    }
}
