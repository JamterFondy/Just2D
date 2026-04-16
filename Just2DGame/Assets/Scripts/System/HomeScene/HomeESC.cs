using UnityEngine;

public class HomeESC : MonoBehaviour
{
    UIManager _uiManager;

    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogWarning("UIManager ‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñBHomeESC ‚Í“®ì‚µ‚Ü‚¹‚ñB");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //if (_uiManager != null && _uiManager.currentState != UIState.Home && _uiManager.currentState == UIState.Map)
            //{
            //    _uiManager.currentState = UIState.Home;
            //}
            //else if (_uiManager != null && _uiManager.currentState != UIState.Home && _uiManager.currentState == UIState.CharacterSelect)
            //{
            //    _uiManager.currentState = UIState.Map;
            //}
        }
    }
}