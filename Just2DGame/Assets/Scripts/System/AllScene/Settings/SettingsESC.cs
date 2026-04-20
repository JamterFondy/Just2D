using UnityEngine;
using UnityEngine.UI;

public class SettingsESC : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        
        titleUIManager.currentState = TitleUIState.Default;
    }
}
