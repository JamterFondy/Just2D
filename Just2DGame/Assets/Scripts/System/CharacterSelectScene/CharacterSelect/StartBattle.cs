using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;

    [SerializeField] GameObject selectCharacter,charaInfoServer;
    

    public int serveID;

    void Start()
    {
        loadingManager = FindObjectOfType<LoadingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
       
        serveID = selectCharacter.GetComponent<SelectCharacter>().characterID;
        charaInfoServer.GetComponent<CharaInfoServer>().ID = serveID;

        charaInfoServer.GetComponent<CharaInfoServer>().SetCharacterInfo();

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "GameScene"));
    }

    
}
