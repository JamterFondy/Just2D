using UnityEngine;

public class TryAgainStage : MonoBehaviour
{
    BattleFinish battleFinish;

    public void OnClick()
    {
       battleFinish = FindAnyObjectByType<BattleFinish>();

        if (battleFinish != null)
        {
            battleFinish.TryAgain();
        }
    }
}
