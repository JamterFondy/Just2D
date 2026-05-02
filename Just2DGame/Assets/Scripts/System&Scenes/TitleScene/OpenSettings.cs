using UnityEngine;
using UnityEngine.UI;

public class OpenSettings : MonoBehaviour
{
    [SerializeField] GameObject target;
    UIManager uiManager;
    ButtonSE buttonSE;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.StateChanged += OnStateChanged;
            UpdateVisibility(uiManager.currentState);
        }
        else
        {
            Debug.LogWarning("UIManager not found. Visibility won't update automatically.");
        }

    }

    void Start()
    {
        buttonSE = FindAnyObjectByType<ButtonSE>();
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.TitleDefault);
    }



    public void OnClick()
    {    
         uiManager.currentState = UIState.Settings;
         buttonSE.PlayButtonSE("Confirm");
    }
}
