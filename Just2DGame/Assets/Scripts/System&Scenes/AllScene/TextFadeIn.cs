using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class TextFadeIn : MonoBehaviour
{
    public TMP_Text tmp;
    public float interval = 0.1f;
　　
    public bool isAllTextAppeared;
    public bool skipRequest; // 外部から受けるテキストのスキップ表示処理。

    public IEnumerator FadeIn()
    {
        isAllTextAppeared = false;

        tmp.ForceMeshUpdate();
        int total = tmp.textInfo.characterCount;

        tmp.maxVisibleCharacters = 0;

        for (int i = 0; i <= total; i++)
        {
            if (skipRequest)
            {
                skipRequest = false;
                i = total;
                tmp.maxVisibleCharacters = total;
            }

            tmp.maxVisibleCharacters = i;
            yield return new WaitForSeconds(interval);
        }

        isAllTextAppeared = true;

        yield break;
    }
}
