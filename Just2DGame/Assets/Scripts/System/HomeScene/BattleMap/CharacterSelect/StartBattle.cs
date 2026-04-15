using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
    [SerializeField] GameObject selectCharacter;
    [SerializeField] IDServer idServer;

    public int serveID;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        serveID = selectCharacter.GetComponent<SelectCharacter>().characterID;

        StartCoroutine(LoadBattleScene());
    }

    IEnumerator LoadBattleScene()
    {
        // シーンの非同期読み込みを開始
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene");
        // 読み込みが完了するまで待機
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
