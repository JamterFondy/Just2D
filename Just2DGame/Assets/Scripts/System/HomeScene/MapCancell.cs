using UnityEngine;
using UnityEngine.UI;
public class MapCancell : MonoBehaviour
{
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
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null) return;
        uiManager.currentState = UIState.Home;
    }
}
