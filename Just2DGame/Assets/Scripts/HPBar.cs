using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] EnemyStatus enemyStatus; // インスペクタで割り当てるか、自動検索される
    [SerializeField] Image image;             // Filled タイプの Image を想定
    [SerializeField] RectTransform fillRect;  // Image を使わない場合に幅で縮める子 RectTransform（任意）
    [SerializeField] bool debugLogs = false;

    int maxHp;
    bool useFillAmount = false;
    float originalFillRectWidth = 0f;

    void Start()
    {
        if (enemyStatus == null)
            enemyStatus = GetComponentInParent<EnemyStatus>();

        if (image == null)
            image = GetComponent<Image>();

        if (enemyStatus == null)
        {
            Debug.LogWarning($"{nameof(HPBar)}: EnemyStatus が見つかりません。インスペクタで割り当ててください。");
            enabled = false;
            return;
        }

        // 優先: Image(Filled) を使う
        if (image != null)
        {
            // スクリプト側で Filled に設定しておく
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            useFillAmount = true;

            if (image.sprite == null && debugLogs)
                Debug.LogWarning($"{nameof(HPBar)}: Image の Source Image が設定されていません。Sprite を割り当ててください。");
        }
        // Image が使えない場合は fillRect を使って幅を縮める
        if (!useFillAmount)
        {
            if (fillRect == null)
            {
                // 同階層・子から "Fill" 名の RectTransform を探す試行
                RectTransform found = transform.Find("Fill") as RectTransform;
                if (found != null) fillRect = found;
            }

            if (fillRect != null)
            {
                originalFillRectWidth = fillRect.rect.width;
                if (originalFillRectWidth <= 0f)
                {
                    Debug.LogWarning($"{nameof(HPBar)}: fillRect の幅が不正です。幅が 0 だと縮小できません。");
                    enabled = false;
                    return;
                }
            }
            else
            {
                Debug.LogWarning($"{nameof(HPBar)}: Image も fillRect も見つかりません。HP 表示を行えません。");
                enabled = false;
                return;
            }
        }

        maxHp = Mathf.Max(1, enemyStatus.hp);
        if (useFillAmount)
            image.fillAmount = 1f;
        else
            SetFillRectWidthRatio(1f);
    }

    void Update()
    {
        if (enemyStatus == null) return;

        float currentHp = enemyStatus.hp;
        float ratio = Mathf.Clamp01(currentHp / (float)maxHp);

        if (useFillAmount)
        {
            image.fillAmount = ratio;
            if (debugLogs) Debug.Log($"[HPBar] fillAmount={image.fillAmount} sprite={(image.sprite != null ? image.sprite.name : "null")}");
        }
        else
        {
            SetFillRectWidthRatio(ratio);
            if (debugLogs) Debug.Log($"[HPBar] fillRect width={(fillRect != null ? fillRect.rect.width : 0)} ratio={ratio}");
        }
    }

    void SetFillRectWidthRatio(float ratio)
    {
        if (fillRect == null) return;
        Vector2 size = fillRect.sizeDelta;
        // anchored の扱いによって sizeDelta が意味する値が変わるが一般的なケースでは幅を直接変更する
        size.x = originalFillRectWidth * ratio;
        fillRect.sizeDelta = size;
    }
}
