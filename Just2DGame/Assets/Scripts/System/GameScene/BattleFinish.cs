using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BattleFinish : MonoBehaviour
{
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

    public void MoveToMapAfterDelay()
    {
        StartCoroutine(MoveToMapCoroutine());
    }

    IEnumerator MoveToMapCoroutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MapScene");
    }
}
