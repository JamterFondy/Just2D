using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleFinish : MonoBehaviour
{
    [SerializeField] GameObject charaInfoServer;
    [SerializeField] LoadingManager loadingManager;

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

        uiManager = FindObjectOfType<UIManager>();
    }

    public void MoveToMapAfterDelay()
    {
        StartCoroutine(MoveToMapCoroutine());
    }

    IEnumerator MoveToMapCoroutine()
    {
        yield return new WaitForSeconds(2f);
        Destroy(charaInfoServer);

        uiManager.currentScene = SceneType.Map;
        uiManager.currentState = UIState.StageMapDefault;

        loadingManager.StartCoroutine(loadingManager.LoadSceneWithLoadingScreen("LoadingScene", "MapScene"));

    }
}
