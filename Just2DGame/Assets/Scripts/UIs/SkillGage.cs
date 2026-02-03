using UnityEngine;
using UnityEngine.UI;

public enum SkillGageState
{
    Empty,
    Charging,
    Full,
}

public class SkillGage : MonoBehaviour
{
    [SerializeField] PlayerStatus playerStatus;

    [SerializeField] Image imageFull;
    [SerializeField] Image imageCharge;

    SkillGageState _currentState = SkillGageState.Empty;
    public SkillGageState currentState
    {
        get => _currentState;
        private set
        {
            if (_currentState == value) return;
            _currentState = value;
            UpdateVisuals(_currentState);
        }
    }

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

        if (imageFull == null)
        {
            Debug.LogWarning("imageFull が未設定です。Full 表示が機能しません。");
        }
        if (imageCharge == null)
        {
            Debug.LogWarning("imageCharge が未設定です。Charging 表示が機能しません。");
        }

        // 初期状態設定（Visualも更新）
        RefreshStateFromPlayer();
    }

    void Update()
    {
        RefreshStateFromPlayer();
    }

    void RefreshStateFromPlayer()
    {
        if (playerStatus == null) return;

        // LeftCrickCTBool が true の間は Charging、false の間は Full にする
        currentState = playerStatus.LeftCrickCTBool ? SkillGageState.Charging : SkillGageState.Full;
    }

    void UpdateVisuals(SkillGageState state)
    {
        // Full のときは imageFull を表示、Charging のときは imageCharge を表示。
        // それ以外は両方非表示にする。
        if (imageFull != null) imageFull.gameObject.SetActive(state == SkillGageState.Full);
        if (imageCharge != null) imageCharge.gameObject.SetActive(state == SkillGageState.Charging);
    }
}