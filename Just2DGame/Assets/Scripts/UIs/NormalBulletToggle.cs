using UnityEngine;
using UnityEngine.UI;

public class NormalBulletToggle : MonoBehaviour
{
    [Tooltip("参照するプレイヤーの PlayerMovement コンポーネント。未設定なら自動で FindObjectOfType します。")]
    [SerializeField] PlayerMovement player;

    [Tooltip("切り替える対象の UI Image。未設定ならこの GameObject の Image を使います。")]
    [SerializeField] Image targetImage;

    [Tooltip("通常弾が出ているときに表示するスプライト")]
    [SerializeField] Sprite normalOnSprite;

    [Tooltip("通常弾が出ていないときに表示するスプライト")]
    [SerializeField] Sprite normalOffSprite;

    bool lastState = false;

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerMovement>();

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (player == null)
            Debug.LogWarning("PlayerMovement が見つかりません。NormalBulletToggle は機能しません。");

        if (targetImage == null)
            Debug.LogWarning("Image コンポーネントが見つかりません。NormalBulletToggle を UI Image にアタッチするか、targetImage を設定してください。");

        // 初期表示を強制更新
        if (player != null && targetImage != null)
        {
            lastState = !player.SpaceToggle; // 強制的に Update で差し替えさせる
        }
    }

    void Update()
    {
        if (player == null || targetImage == null)
            return;

        bool current = player.SpaceToggle;
        if (current == lastState)
            return;

        targetImage.sprite = current ? normalOnSprite : normalOffSprite;
        lastState = current;
    }
}
