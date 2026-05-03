using UnityEngine;

public class QuitStage : MonoBehaviour
{
    BattleFinish battleFinish;

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void OnClick()
    {
        battleFinish = FindAnyObjectByType<BattleFinish>();

        battleFinish.MoveToMapAfterDelay(); //ToDo ESCが有効だとコルーチンによるシーン切り替えができない。すぐにシーン移動する形に切り替えるか、またはBattleESCの参照を取ってIsPosedを強制的に解除（false）にする。
    }
}
