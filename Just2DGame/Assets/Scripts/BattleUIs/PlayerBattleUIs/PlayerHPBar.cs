using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus; // インスペクタで割り当てるか、自動検索される
    [SerializeField] Image image;             // Filled タイプの Image を想定
    [SerializeField] RectTransform fillRect;  // Image を使わない場合に幅で縮める子 RectTransform（任意）
    [SerializeField] bool debugLogs = false;

    public int maxHp;
    int currentHp;
    int pre_currentHp;

    [Header("Color Transition")]
    [Tooltip("How fast the HP bar color interpolates to the target color (higher is faster)")]
    [SerializeField] float colorLerpSpeed = 6f;
    Color currentColor;

    [Header("Clamp Visual")]
    [Tooltip("Duration (seconds) to animate the displayed fill from previous value to new HP ratio")]
    [SerializeField] float clampDuration = 0.3f;

    // internal clamp state (for smooth clamp animation of displayed fill)
    bool isClamping = false;
    float clampTimer = 0f;
    float clampStart = 0f;
    float clampTarget = 0f;
    float displayedRatio = 1f; // this drives what is shown on screen

    bool useFillAmount = false;
    float originalFillRectWidth = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerStatus == null)
            playerStatus = GetComponentInParent<PlayerStatus>();


        if (image == null)
            image = GetComponent<Image>();


        if (playerStatus == null)
        {
            Debug.LogWarning($"{nameof(PlayerHPBar)}: PlayerStatus が見つかりません。インスペクタで割り当ててください。");
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

        // 内部変数HPの初期化
        maxHp = playerStatus.hp;
        currentHp = maxHp;

        if (useFillAmount)
            image.fillAmount = 1f;
        else
            SetFillRectWidthRatio(1f);

        // initialize currentColor from image
        currentColor = image != null ? image.color : Color.white;

        // initialize displayedRatio
        displayedRatio = 1f;
    }

    

    void Update()
    {           
        currentHp = playerStatus.hp;
        
        float ratio = Mathf.Clamp01((float)currentHp / (float)maxHp);

        // We animate the displayed fill (displayedRatio) towards the actual ratio when HP decreases.
        if (useFillAmount)
        {
            // detect decrease
            if (ratio < displayedRatio)
            {
                // start or restart clamp animation toward new ratio
                clampStart = displayedRatio;
                clampTarget = ratio;
                clampTimer = 0f;
                isClamping = true;
            }

            if (isClamping)
            {
                clampTimer += Time.deltaTime;
                float tt = clampDuration <= 0f ? 1f : Mathf.Clamp01(clampTimer / clampDuration);
                displayedRatio = Mathf.Lerp(clampStart, clampTarget, tt);
                image.fillAmount = displayedRatio;

                if (tt >= 1f)
                {
                    isClamping = false;
                }
            }
            else
            {
                // immediate snap when HP increases
                if (ratio > displayedRatio)
                {
                    displayedRatio = ratio;
                }
                image.fillAmount = displayedRatio;
            }
        }
        else
        {
            // when using rect width, animate similarly by adjusting width according to displayedRatio
            if (ratio < displayedRatio)
            {
                clampStart = displayedRatio;
                clampTarget = ratio;
                clampTimer = 0f;
                isClamping = true;
            }

            if (isClamping)
            {
                clampTimer += Time.deltaTime;
                float tt = clampDuration <= 0f ? 1f : Mathf.Clamp01(clampTimer / clampDuration);
                displayedRatio = Mathf.Lerp(clampStart, clampTarget, tt);
                SetFillRectWidthRatio(displayedRatio);

                if (tt >= 1f)
                {
                    isClamping = false;
                }
            }
            else
            {
                if (ratio > displayedRatio)
                {
                    displayedRatio = ratio;
                }
                SetFillRectWidthRatio(displayedRatio);
            }
        }

        // Color change starts when currentHp/maxHp < 0.75
        float ratioHp = (float)currentHp / Mathf.Max(1, maxHp);
        float blue = 140f / 255f;
        Color col;
        if (ratioHp >= 0.75f)
        {
            // no red, full green until threshold
            col = new Color(0f, 1f, blue, image.color.a);
        }
        else
        {
            // remap ratio so that 0.75 -> 1.0, 0 -> 0.0: adj = ratioHp * (4/3)
            float adj = Mathf.Clamp01(ratioHp * (4f / 3f));
            float red = 1f - adj;
            float green = adj;
            col = new Color(Mathf.Clamp01(red), Mathf.Clamp01(green), blue, image.color.a);
        }
        // smooth transition to target color
        if (currentColor == default(Color)) currentColor = image.color;
        float t = Mathf.Clamp01(colorLerpSpeed * Time.deltaTime);
        currentColor = Color.Lerp(currentColor, col, t);
        if (image != null) image.color = currentColor;
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
