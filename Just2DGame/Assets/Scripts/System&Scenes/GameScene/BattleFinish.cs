using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleFinish : MonoBehaviour
{
    [SerializeField] GameObject charaInfoServer;
    [SerializeField] LoadingManager loadingManager;
    [SerializeField] NextStageLoader nextStageLoader;

    UIManager uiManager;

    static BattleFinish _instance;
    public static BattleFinish Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("BattleFinish");
                _instance = go.AddComponent<BattleFinish>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance == null) _instance = this;
        else if (_instance != this) Destroy(gameObject);
    }

    void Start()
    {
        charaInfoServer = GameObject.Find("CharaInfoServer");
        loadingManager = FindAnyObjectByType<LoadingManager>();
        nextStageLoader = FindAnyObjectByType<NextStageLoader>();

        uiManager = FindAnyObjectByType<UIManager>();
    }

    public IEnumerator BossDied(GameObject bossEnemy)
    {

        // 撃破アニメーションを挟むなど

        yield return new WaitForSeconds(1f);

        Destroy(bossEnemy);

        uiManager.currentState = UIState.PlayerWin;
    }

    public IEnumerator PlayerDied()
    {
        yield return new WaitForSeconds(0.5f);

        uiManager.currentState = UIState.PlayerLose;
    }


    public void MoveToMapAfterDelay()
    {
        StartCoroutine(MoveToMapCoroutine());
    }

    IEnumerator MoveToMapCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(charaInfoServer);

        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));
    }

    public void QuitStage()
    {
        Destroy(charaInfoServer);

        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));
    }

    public void TryAgain()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "GameScene"));
    }

    public IEnumerator GoNextStage()
    {
        uiManager.currentScene = SceneType.Loading;
        uiManager.currentState = UIState.Loading;

        if(nextStageLoader == null) nextStageLoader = FindAnyObjectByType<NextStageLoader>();
        if(loadingManager == null) loadingManager = FindAnyObjectByType<LoadingManager>();

        bool isLoadDone = nextStageLoader.ChangeLayout_NextStage(false);

        yield return new WaitUntil(() => isLoadDone);

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "GameScene"));
    }
}
