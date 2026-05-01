using UnityEngine;
using System.Collections;

public class BattleESC : MonoBehaviour
{
    [Header("再開遅延（秒）")]
    [SerializeField] float resumeDelay = 3f;

    public bool isPaused = false;
    Coroutine resumeCoroutine;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                // ポーズ中：再開カウントをトグル（未開始なら開始、開始中ならキャンセル）
                if (resumeCoroutine == null)
                    resumeCoroutine = StartCoroutine(ResumeAfterDelayRealtime(Mathf.Max(0f, resumeDelay)));
                else
                {
                    StopCoroutine(resumeCoroutine);
                    resumeCoroutine = null;
                    Debug.Log("Resume cancelled. Still paused.");
                }
            }
        }
    }

    void PauseGame()
    {
        // TimeScale を 0 にして物理/時間依存処理を止める（MonoBehaviour の enabled は変更しない）
        Time.timeScale = 0f;
        AudioListener.pause = true;

        isPaused = true;
        Debug.Log("Game paused. Press ESC again to start " + resumeDelay + "s resume timer.");
    }

    IEnumerator ResumeAfterDelayRealtime(float delay)
    {
        Debug.Log("Resume will occur in " + delay + " seconds (real time)...");
        // real-time wait so Time.timeScale = 0 の間もカウントされる
        yield return new WaitForSecondsRealtime(delay);

        // 再開
        Time.timeScale = 1f;
        AudioListener.pause = false;

        isPaused = false;
        resumeCoroutine = null;
        Debug.Log("Game resumed.");
    }

    void OnDestroy()
    {
        // 念のため復帰処理（破棄時に TimeScale が 0 のままにならないよう）
        if (resumeCoroutine != null) StopCoroutine(resumeCoroutine);
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
        }
    }
}