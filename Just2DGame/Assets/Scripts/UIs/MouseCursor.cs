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
    }

    void Update()
    {
        if (rectTransform == null)
            return;

        Vector2 screenPoint = Input.mousePosition;

        if (parentCanvas != null)
        {
            RectTransform canvasRect = parentCanvas.transform as RectTransform;
            Vector2 localPoint;
            Camera cam = parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, cam, out localPoint))
            {
                rectTransform.anchoredPosition = localPoint;
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
