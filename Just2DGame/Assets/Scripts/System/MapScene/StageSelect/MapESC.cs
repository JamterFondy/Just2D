using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapESC : MonoBehaviour
{
    [SerializeField] MapUIManager mapUIManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mapUIManager = FindObjectOfType<MapUIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
       if(mapUIManager.currentState == MapUIState.MapSelect)
       {
           mapUIManager.currentState = MapUIState.Map;
       }
       else if(mapUIManager.currentState == MapUIState.Map)
       {
            SceneManager.LoadScene("HomeScene");
       }
    }
}
