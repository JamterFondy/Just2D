using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    [SerializeField] LoadingManager loadingManager;

    [SerializeField] GameObject selectCharacter,charaInfoServer;

    UIManager uiManager;
    

    public int serveID;

    void Start()
    {
        loadingManager = FindAnyObjectByType<LoadingManager>();
        uiManager = FindAnyObjectByType<UIManager>();

        if (PlayerPrefs.HasKey("GoBattleCharacterID")) serveID = PlayerPrefs.GetInt("GoBattleCharacterID");
    }

    // Update is called once per frame
    void Update()
    {
         
    }

    public void OnClick()
    {
        serveID = selectCharacter.GetComponent<SelectCharacter>().characterID;
        if (serveID == 0) return; //キャラが選択されていないならバトルに行けない
      
        PlayerPrefs.SetInt("GoBattleCharacterID", serveID); //選択したキャラのIDを保存。次回はこの番号のキャラが選択されている状態になる。


        charaInfoServer.GetComponent<CharaInfoServer>().ID = serveID;

        charaInfoServer.GetComponent<CharaInfoServer>().SetCharacterInfo();

        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "GameScene"));
    }

    
}
