using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoToBattleMap : MonoBehaviour
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

        gameObject.SetActive(uiManager.currentState == UIState.Home);
      
    }

    public void OnClick()
    {
        SceneManager.LoadScene("MapScene");
    } 
}
