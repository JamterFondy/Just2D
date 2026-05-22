using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using System;

public class TextFadeIn : MonoBehaviour
{
    [SerializeField] SEManager seManager;

    public TMP_Text tmp;
    public float interval = 0.1f;
　　
    public bool isAllTextAppeared;
    public bool isTextVoiceContinue;
    public bool skipRequest; // 外部から受けるテキストのスキップ表示処理。

    void Awake()
    {
        if (seManager == null)
        {
            seManager = GetComponent<SEManager>();
        }
    }

    public IEnumerator FadeIn(string charaName)
    {
        isAllTextAppeared = false;
        isTextVoiceContinue = true;

        string splitedCharaName = charaName.Split('_')[0];

        StartCoroutine(PlayTextVoice(charaName, splitedCharaName));

        tmp.ForceMeshUpdate();
        int totalCharacters = tmp.textInfo.characterCount;

        tmp.maxVisibleCharacters = 0;

        for (int i = 0; i <= totalCharacters; i++)
        {
            if (skipRequest)
            {
                skipRequest = false;
                isTextVoiceContinue = false;
                i = totalCharacters;
                tmp.maxVisibleCharacters = totalCharacters;
            }

            tmp.maxVisibleCharacters = i;

            if(i == totalCharacters)
            {
                isTextVoiceContinue = false;
            }

            yield return new WaitForSeconds(interval);
        }


        isAllTextAppeared = true;

        yield break;
    }

    IEnumerator PlayTextVoice(string charaName, string splitedCharaName)
    {
        while (isTextVoiceContinue)
        {
            seManager.PlaySE("TextVoice", charaName, splitedCharaName);
            yield return new WaitForSeconds(0.125f);
        }
        yield break;
    }
}
