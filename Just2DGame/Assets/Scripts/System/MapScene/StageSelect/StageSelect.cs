using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageSelect : MonoBehaviour
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

    }

    public void OnClick()
    {
        mapUIManager.currentState = MapUIState.MapSelect;
    }
}
