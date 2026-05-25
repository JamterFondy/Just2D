using TMPro;
using UnityEngine;

public class StageQuestPannel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mainQuest;
    [SerializeField] TextMeshProUGUI[] subQuest;

  　public void SetQuestText(string main, string[] sub)
    {
        mainQuest.text = main;

        for (int i = 0; i < sub.Length && i < subQuest.Length; i++)
        {
            subQuest[i].text = sub[i];
        }

    }
}
