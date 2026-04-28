using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SkillGage : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    void Start()
    {
        if (playerStatus == null)
        {
            playerStatus = FindObjectOfType<PlayerStatus>();
            if (playerStatus == null)
            {
                Debug.LogWarning("PlayerStatus が見つかりません。SkillGage は動作しません。");
            }
        }
    }

    public void SetCoolTimeImage(Sprite sprite, float coolTime)
    {
        Image image = GetComponent<Image>();
        if (image == null) return;

        // まず全体を暗くする
        image.sprite = sprite;
        image.color = new Color(0.3f, 0.3f, 0.3f, 1f); // 暗く表示

        // マスク用Imageを生成または取得
        Image maskImage = GetOrCreateMaskImage();

        // コルーチンでクールタイム演出
        StartCoroutine(CoolTimeEffectCoroutine(image, maskImage, sprite, coolTime));
    }

    private Image GetOrCreateMaskImage()
    {
        // 子に"SkillGageMask"があればそれを使う。なければ新規作成
        Transform maskTrans = transform.Find("SkillGageMask");
        Image maskImage;
        if (maskTrans != null)
        {
            maskImage = maskTrans.GetComponent<Image>();
        }
        else
        {
            GameObject maskObj = new GameObject("SkillGageMask");
            maskObj.transform.SetParent(transform, false);
            maskImage = maskObj.AddComponent<Image>();
            maskImage.raycastTarget = false;
        }
        // 親と同じサイズ・位置に合わせる
        RectTransform parentRect = GetComponent<RectTransform>();
        RectTransform maskRect = maskImage.GetComponent<RectTransform>();
        maskRect.anchorMin = Vector2.zero;
        maskRect.anchorMax = Vector2.one;
        maskRect.offsetMin = Vector2.zero;
        maskRect.offsetMax = Vector2.zero;
        maskImage.sprite = GetComponent<Image>().sprite;
        maskImage.type = Image.Type.Filled;
        maskImage.fillMethod = Image.FillMethod.Radial360;
        maskImage.fillOrigin = (int)Image.Origin360.Top; // 12時方向
        maskImage.fillClockwise = false; // 反時計回り
        maskImage.color = Color.white; // 明るい（白）
        maskImage.fillAmount = 0f; // 最初は全て暗い（明るい部分なし）
        return maskImage;
    }

    private IEnumerator CoolTimeEffectCoroutine(Image baseImage, Image maskImage, Sprite sprite, float coolTime)
    {
        float timer = 0f;
        maskImage.enabled = true;
        maskImage.sprite = sprite;
        maskImage.fillAmount = 0f; // 最初は全て暗い
        maskImage.fillMethod = Image.FillMethod.Radial360;
        maskImage.fillOrigin = (int)Image.Origin360.Top; // 12時方向
        maskImage.fillClockwise = false; // 反時計回り

        // 滑らかに明るくする
        while (timer < coolTime)
        {
            timer += Time.deltaTime;
            float filled = Mathf.Clamp01(timer / Mathf.Max(0.0001f, coolTime));
            maskImage.fillAmount = filled;
            yield return null;
        }
        maskImage.fillAmount = 1f;

        // クールタイム終了時、全体を明るく戻す
        baseImage.color = Color.white;
        maskImage.enabled = false;
    }

    public void SetFullImage(Sprite fullImage)
    {
        Image image = GetComponent<Image>();
        image.sprite = fullImage;
    }
}