using UnityEngine;
using UnityEngine.UI;

public class NormalBulletToggle : MonoBehaviour
{
    [Tooltip("参照するプレイヤーの Chara1NormalBullet コンポーネント。未設定なら自動で FindObjectOfType します。")]
    [SerializeField] Chara1NormalBullet chara1NormalBullet;

    [Tooltip("切り替える対象の UI Image。未設定ならこの GameObject の Image を使います。")]
    [SerializeField] Image targetImage;

    [Tooltip("通常弾が出ているときに表示するスプライト")]
    [SerializeField] Sprite normalOnSprite;

    [Tooltip("通常弾が出ていないときに表示するスプライト")]
    [SerializeField] Sprite normalOffSprite;

    bool lastState = false;

    void Start()
    {
        if (chara1NormalBullet == null)
            chara1NormalBullet = FindObjectOfType<Chara1NormalBullet>();

        if (targetImage == null)
            targetImage = GetComponent<Image>();

        if (chara1NormalBullet == null)
            Debug.LogWarning("Chara1NormalBullet が見つかりません。NormalBulletToggle は機能しません。");

        if (targetImage == null)
            Debug.LogWarning("Image コンポーネントが見つかりません。NormalBulletToggle を UI Image にアタッチするか、targetImage を設定してください。");

        // 初期表示を強制更新
        if (chara1NormalBullet != null && targetImage != null)
        {
            lastState = !chara1NormalBullet.SpaceToggle; // 強制的に Update で差し替えさせる
        }
    }

    void Update()
    {
        if (chara1NormalBullet == null || targetImage == null)
            return;

        bool current = chara1NormalBullet.SpaceToggle;
        if (current == lastState)
            return;

        targetImage.sprite = current ? normalOnSprite : normalOffSprite;
        lastState = current;
    }
}
