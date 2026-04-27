using UnityEngine;
using UnityEngine.UI;
public class SkillGage : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    Image imageFull; // ゲージが満タンのときの画像
    Image imageCharge; // ゲージがチャージ中のときの画像

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

    public void SetImage(Sprite sprite)
    {
        Image image = GetComponent<Image>();

        image.sprite = sprite;
    }

}