using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class TextFadeIn : MonoBehaviour
{
    public TMP_Text tmp;
    public float interval = 0.1f;

    public IEnumerator FadeIn()
    {
        tmp.ForceMeshUpdate();
        int total = tmp.textInfo.characterCount;

        tmp.maxVisibleCharacters = 0;

        for (int i = 0; i <= total; i++)
        {
            tmp.maxVisibleCharacters = i;
            yield return new WaitForSeconds(interval);
        }
        

        yield break;
    }
}
