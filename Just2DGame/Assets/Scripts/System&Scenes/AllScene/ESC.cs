using UnityEngine;

public class ESC : MonoBehaviour
{
    SEManager seManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                if (UIManager.Instance.currentState == UIState.Settings)
                {
                    UIManager.Instance.currentState = UIState.TitleDefault;
                    seManager.PlaySE("Button", "Cancel_Button", "Cancel");
                    return;
                }
                else if (UIManager.Instance.currentState == UIState.GameStartConfirm)
                {
                    UIManager.Instance.currentState = UIState.TitleDefault;
                    seManager.PlaySE("Button", "Cancel_Button", "Cancel");
                    return;
                }
                else if (UIManager.Instance.currentState == UIState.Credit)
                {
                    UIManager.Instance.currentState = UIState.TitleDefault;
                    seManager.PlaySE("Button", "Cancel_Button", "Cancel");
                    return;

                }
            
        }
    }
}
