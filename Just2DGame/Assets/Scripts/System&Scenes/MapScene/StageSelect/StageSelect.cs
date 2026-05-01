using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    UIManager uiManager;
    StageSelectManager stageSelectManager;

    string objName;
    int stageNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        stageSelectManager = FindObjectOfType<StageSelectManager>();

        objName = gameObject.name;

        Match match = Regex.Match(objName, @"\d+$");

        if (match.Success)
        {
            stageNum = int.Parse(match.Value);
            Debug.Log("末尾の数字: " + stageNum);
        }
        else
        {
            Debug.Log("末尾に数字がありません");
        }

    }


    public void OnClick()
    {
        uiManager.currentState = UIState.StageInfo;
        stageSelectManager.stageNum = stageNum;
    }
}
