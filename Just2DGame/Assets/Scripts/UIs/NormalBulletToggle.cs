using UnityEngine;
using UnityEngine.UI;

public class NormalBulletToggle : MonoBehaviour
{
    [Tooltip("参照するプレイヤーの NormalBullet コンポーネント。未設定なら自動で FindObjectOfType します。")]
    [SerializeField] NormalBullet normalBullet;

    [Tooltip("切り替える対象の UI Image。未設定ならこの GameObject の Image を使います。")]
    [SerializeField] Image targetImage;

    [Tooltip("通常弾が出ているときに表示するスプライト")]
    [SerializeField] Sprite normalOnSprite;

    [Tooltip("通常弾が出ていないときに表示するスプライト")]
    [SerializeField] Sprite normalOffSprite;

    bool lastState = false;

    void Start()
    {
        if (normalBullet == null)
            normalBullet = FindObjectOfType<NormalBullet>();

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (normalBullet == null)
            Debug.LogWarning("NormalBullet が見つかりません。NormalBulletToggle は機能しません。");

        if (targetImage == null)
            Debug.LogWarning("Image コンポーネントが見つかりません。NormalBulletToggle を UI Image にアタッチするか、targetImage を設定してください。");

        // 初期表示を強制更新
        if (normalBullet != null && targetImage != null)
        {
            lastState = !normalBullet.SpaceToggle; // 強制的に Update で差し替えさせる
        }
    }

    void Update()
    {
        if (normalBullet == null || targetImage == null)
            return;

        bool current = normalBullet.SpaceToggle;
        if (current == lastState)
            return;

        targetImage.sprite = current ? normalOnSprite : normalOffSprite;
        lastState = current;
    }
}
