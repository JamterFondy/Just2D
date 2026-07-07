using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WinPannel : MonoBehaviour
{
    StageLoader stageLoader;
    BattleFinish battleFinish;
    [SerializeField] GameObject target;

    string nextStageName;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        stageLoader = FindAnyObjectByType<StageLoader>();

        
        UIManager.Instance.StateChanged += OnStateChanged;
        UpdateVisibility(UIManager.Instance.currentState);
    }

    void OnDestroy()
    {
        UIManager.Instance.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.PlayerWin);

        if (stageLoader == null)
        {
            stageLoader = FindAnyObjectByType<StageLoader>();
        }

        // 次のステージ名を計算し、テキストに受け渡す
        string currentStageName = stageLoader.stageName;
        if (currentStageName != null)
        {
            int currentStageNum = int.Parse(currentStageName.Replace("-", ""));

            Debug.Log($"{currentStageNum}");


            if ((currentStageNum + 1) % 10 == 0)
            {
                int nextStageNum = currentStageNum + 2;
                nextStageName = string.Join("-", nextStageNum.ToString().ToCharArray());
            }
            else
            {
                int nextStageNum = currentStageNum + 1;
                nextStageName = string.Join("-", nextStageNum.ToString().ToCharArray());
            }
        }
    }


    public void GoNextStage()
    {
       if(battleFinish == null) battleFinish = FindAnyObjectByType<BattleFinish>();

       battleFinish.StartCoroutine(battleFinish.GoNextStage());
    }


    public void GoBackMap()
    {
        if (battleFinish == null) battleFinish = FindAnyObjectByType<BattleFinish>();
        battleFinish.QuitStage();
    }
}
