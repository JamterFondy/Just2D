using UnityEngine;

public class ESC : MonoBehaviour
{
    [SerializeField] TitleUIManager titleUIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleUIManager = FindObjectOfType<TitleUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            if (titleUIManager != null)
            {
                if (titleUIManager.currentState == TitleUIState.Settings)
                {
                    titleUIManager.currentState = TitleUIState.Default;
                    return;
                }
                else if (titleUIManager.currentState == TitleUIState.StartConfirm)
                {
                    titleUIManager.currentState = TitleUIState.Default;
                    return;
                }
                else if (titleUIManager.currentState == TitleUIState.Credit)
                {
                    titleUIManager.currentState = TitleUIState.Default;
                    return;

                }
            }
        }
    }
}
