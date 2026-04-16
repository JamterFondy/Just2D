using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MouseCursorCanvas : MonoBehaviour
{
    public int sortingOrder = 1000;
    // Desired render mode for the persistent cursor canvas. Set to WorldSpace to use world-space UI.
    public RenderMode desiredRenderMode = RenderMode.WorldSpace;
    // When using WorldSpace, distance from camera to place the canvas (in meters)
    public float worldDistance = 2f;
    // If true and a main camera exists, align the canvas to face and follow the camera on scene load
    public bool attachToCamera = true;
    static MouseCursorCanvas instance;
    Canvas canvas;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        // Ensure we mark the root GameObject as persistent so the whole UI root is preserved
        var root = transform.root != null ? transform.root.gameObject : gameObject;
        DontDestroyOnLoad(root);

        canvas = GetComponentInParent<Canvas>() ?? GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;
        }

        var img = GetComponentInChildren<Image>();
        if (img != null) img.raycastTarget = false;
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 新しいシーンの UI の上に来るように最後尾に移動
        transform.SetAsLastSibling();

        // 再取得して Canvas 設定を確実にする（RenderMode 等がシーンに依存して変わる場合があるため）
        canvas = GetComponentInChildren<Canvas>() ?? GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;

            // Set desired render mode (WorldSpace / ScreenSpaceCamera / ScreenSpaceOverlay)
            canvas.renderMode = desiredRenderMode;

            if (desiredRenderMode == RenderMode.WorldSpace)
            {
                // For WorldSpace, set camera and position the canvas in front of the main camera if requested
                var cam = Camera.main;
                canvas.worldCamera = cam;
                if (attachToCamera && cam != null)
                {
                    var cT = canvas.transform;
                    cT.position = cam.transform.position + cam.transform.forward * worldDistance;
                    cT.rotation = cam.transform.rotation;
                }
            }
            else if (desiredRenderMode == RenderMode.ScreenSpaceCamera)
            {
                canvas.worldCamera = Camera.main;
            }
            else
            {
                canvas.worldCamera = null;
            }
        }

        Canvas.ForceUpdateCanvases();

        // 子の MouseCursor を再初期化して座標基準を正しくする
        var cursors = GetComponentsInChildren<MouseCursor>(true);
        foreach (var c in cursors)
        {
            c.RefreshCanvasReference();
        }
    }
}