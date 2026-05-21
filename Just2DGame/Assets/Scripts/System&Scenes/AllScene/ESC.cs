using UnityEngine;

public class ESC : MonoBehaviour
{
    UIManager uiManager;
    SEManager seManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindAnyObjectByType<UIManager>();
        seManager = FindAnyObjectByType<SEManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            if (uiManager != null)
            {
                if (uiManager.currentState == UIState.Settings)
                {
                    uiManager.currentState = UIState.TitleDefault;
                    seManager.PlaySE("Button", "Cancel_Button");
                    return;
                }
                else if (uiManager.currentState == UIState.GameStartConfirm)
                {
                    uiManager.currentState = UIState.TitleDefault;
                    seManager.PlaySE("Button", "Cancel_Button");
                    return;
                }
                else if (uiManager.currentState == UIState.Credit)
                {
                    uiManager.currentState = UIState.TitleDefault;
                    seManager.PlaySE("Button", "Cancel_Button");
                    return;

                }
            }
        }
    }
}
