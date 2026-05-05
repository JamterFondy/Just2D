using UnityEngine;

public class QuitStageSelect : MonoBehaviour
{
    UIManager uiManager;

    public void OnClick()
    {
        uiManager = FindAnyObjectByType<UIManager>();

        uiManager.currentState = UIState.BattleQuitConfirm;
    }

}
