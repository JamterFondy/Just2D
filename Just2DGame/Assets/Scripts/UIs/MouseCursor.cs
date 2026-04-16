using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    RectTransform rectTransform;
    Canvas parentCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentCanvas = GetComponentInParent<Canvas>();

        if (rectTransform == null)
            Debug.LogError("MouseCursor: RectTransform が見つかりません。UI の Image にアタッチしてください。");

        if (parentCanvas == null)
            Debug.LogWarning("MouseCursor: 親に Canvas が見つかりません。UI レイヤー外での動作は制限されます。");

        // カーソル Image が UI のクリックを妨げないようにする
        var img = GetComponent<Image>();
        if (img != null) img.raycastTarget = false;
    }

    // シーン切り替えなどで Canvas 構成が変わったときに呼び出して
    // Canvas 参照を再取得し、カーソル位置を再計算します。
    public void RefreshCanvasReference()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        // Image の raycast を再確認
        var img = GetComponent<Image>();
        if (img != null) img.raycastTarget = false;

        // 再配置
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (rectTransform == null)
            return;

        Vector2 screenPoint = Input.mousePosition;

        if (parentCanvas != null)
        {
            RectTransform canvasRect = parentCanvas.transform as RectTransform;
            Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

            if (parentCanvas.renderMode == RenderMode.WorldSpace)
            {
                Vector3 worldPoint;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPoint, cam, out worldPoint))
                {
                    rectTransform.position = worldPoint;
                }
            }
            else
            {
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, cam, out localPoint))
                {
                    rectTransform.anchoredPosition = localPoint;
                }
            }
        }
        else
        {
            Vector3 worldPos = Camera.main != null ? Camera.main.ScreenToWorldPoint(screenPoint) : Vector3.zero;
            worldPos.z = 0f;
            transform.position = worldPos;
        }
    }

    void Start()
    {
        // マウスカーソルを非表示にする
        Cursor.visible = false;

    }

    void Update()
    {
        if (rectTransform == null)
            return;

        Vector2 screenPoint = Input.mousePosition;
        if (parentCanvas != null)
        {
            RectTransform canvasRect = parentCanvas.transform as RectTransform;
            Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

            if (parentCanvas.renderMode == RenderMode.WorldSpace)
            {
                // WorldSpace Canvas: convert screen point to world point on the canvas rect
                Vector3 worldPoint;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvasRect, screenPoint, cam, out worldPoint))
                {
                    rectTransform.position = worldPoint;
                }
            }
            else
            {
                // ScreenSpace Overlay/Camera: convert to local point (anchoredPosition)
                Vector2 localPoint;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, cam, out localPoint))
                {
                    rectTransform.anchoredPosition = localPoint;
                }
            }
        }
        else
        {
            // Canvas が無い場合のフォールバック（ワールド座標に変換）
            Vector3 worldPos = Camera.main != null ? Camera.main.ScreenToWorldPoint(screenPoint) : Vector3.zero;
            worldPos.z = 0f;
            transform.position = worldPos;
        }
    }

}
