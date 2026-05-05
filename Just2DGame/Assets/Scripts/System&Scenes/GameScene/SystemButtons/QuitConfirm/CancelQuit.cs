using UnityEngine;

public class CancelQuit : MonoBehaviour
{
    UIManager uiManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Onclick();
        }
    }
    public void Onclick()
    {
        uiManager = FindAnyObjectByType<UIManager>();

        uiManager.currentState = UIState.BattlePauseMenu;
    }
    
}
