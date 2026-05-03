using UnityEngine;

public class BattleSettingsButton : MonoBehaviour
{
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        uiManager = FindAnyObjectByType<UIManager>();

        uiManager.currentState = UIState.Settings;
    }
}
