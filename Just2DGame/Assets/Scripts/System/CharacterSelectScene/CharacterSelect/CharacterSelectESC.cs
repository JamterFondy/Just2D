using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectESC : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))   
       {
           SceneManager.LoadScene("MapScene");
       }
    }

    public void OnClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
