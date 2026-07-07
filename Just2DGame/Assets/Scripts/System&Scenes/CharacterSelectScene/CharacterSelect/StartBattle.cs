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
        loadingManager = FindAnyObjectByType<LoadingManager>();

        if (PlayerPrefs.HasKey("GoBattleCharacterID")) serveID = PlayerPrefs.GetInt("GoBattleCharacterID");
    }


    public void OnClick()
    {
        serveID = selectCharacter.GetComponent<SelectCharacter>().characterID;
        if (serveID == 0) return; //キャラが選択されていないならバトルに行けない
      
        PlayerPrefs.SetInt("GoBattleCharacterID", serveID); //選択したキャラのIDを保存。次回はこの番号のキャラが選択されている状態になる。


        charaInfoServer.GetComponent<CharaInfoServer>().ID = serveID;

        charaInfoServer.GetComponent<CharaInfoServer>().SetCharacterInfo();

        UIManager.Instance.currentScene = SceneType.Loading;
        UIManager.Instance.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "GameScene"));
    }

    
}
