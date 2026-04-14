using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] UIManager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager == null) return;

        gameObject.SetActive(uiManager.currentState == UIState.Map);

    }

    public void OnClick()
    {
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null) return;

        uiManager.currentState = UIState.CharacterSelect;
    }
}
