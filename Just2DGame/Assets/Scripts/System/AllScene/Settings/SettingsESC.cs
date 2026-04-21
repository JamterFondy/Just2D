using UnityEngine;
using UnityEngine.UI;

public class SettingsESC : MonoBehaviour
{
    UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        
        uiManager.currentState = UIState.TitleDefault;
    }
}
