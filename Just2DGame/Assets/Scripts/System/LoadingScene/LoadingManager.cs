using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    UIManager uiManager;

    public float loadTimeout = 30f; // seconds to wait before considering load failed

    void Start()
    {
        // Persist the GameObject so the loading coroutine survives scene changes
        DontDestroyOnLoad(gameObject);
        uiManager = FindObjectOfType<UIManager>();
    }

    // LoadingScene を先に読み込んでから target を読み込む例
    public IEnumerator LoadSceneWithLoadingScreen(string loadingSceneName, string targetSceneName)
    {
        // Remember currently active scene so we can temporarily deactivate it while loading
        Scene prevScene = SceneManager.GetActiveScene();
        // Variables to track deactivated root objects and timeouts
        var deactivatedRoots = new System.Collections.Generic.List<GameObject>();
        float elapsed = 0f;
        Scene loadingSceneForUnload = default;

        // 1) Loading シーンを Additive で読み込みして表示
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Additive);

        // After loading the loading scene, make it the active scene and deactivate the previous scene's root objects
        Scene loadingScene = SceneManager.GetSceneByName(loadingSceneName);
        loadingSceneForUnload = loadingScene;
        if (loadingScene.IsValid() && loadingScene.isLoaded)
        {
            // Set loading scene active so its UI receives events
            SceneManager.SetActiveScene(loadingScene);

            // Deactivate all root objects in the previous scene to prevent their UI/scripts from running or receiving input
            if (prevScene.IsValid())
            {
                try
                {
                    var roots = prevScene.GetRootGameObjects();
                    foreach (var go in roots)
                    {
                        // Skip null safety
                        if (go == null) continue;
                        // Do not attempt to deactivate objects that are in a different scene or are DontDestroyOnLoad (they won't be in prevScene roots)
                        if (go.activeSelf)
                        {
                            go.SetActive(false);
                            deactivatedRoots.Add(go);
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"LoadingManager: Failed to deactivate previous scene roots: {ex}");
                }
            }
        }
        // ここで Loading シーンにある UI を取得して進捗表示を更新する準備をする

        // 2) ターゲットシーンを非同期で読み込み開始（自動アクティベートしない）
        AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneName);
        op.allowSceneActivation = false;

        // 3) 読み込み進捗を監視して UI を更新
        while (op.progress < 0.9f)
        {
            float normalized = Mathf.Clamp01(op.progress / 0.9f); // 0..1 に正規化
            // 進捗バー等を更新する（例: SetProgress(normalized)）
            yield return null;
        }

        // 4) 読み込みが完了 (progress は 0.9) — 必要ならここでロード完了表示や短い待ちを行う
        // SetProgress(1f); // 100% 表示
        // 任意のアニメーション/フェードを待つ
        yield return new WaitForSeconds(0.5f);

        // 5) シーンをアクティベート
        op.allowSceneActivation = true;

        // 6) アクティベートが完了するのを待つ (with timeout)
        elapsed = 0f;
        while (!op.isDone)
        {
            elapsed += Time.unscaledDeltaTime;
            if (elapsed > loadTimeout)
            {
                Debug.LogError($"LoadingManager: Scene activation timed out after {loadTimeout} seconds: {targetSceneName}");
                // Restore prev scene roots
                if (prevScene.IsValid())
                {
                    try
                    {
                        SceneManager.SetActiveScene(prevScene);
                        foreach (var g in deactivatedRoots)
                        {
                            if (g != null)
                                g.SetActive(true);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Debug.LogWarning($"LoadingManager: Failed to restore previous scene roots after activation timeout: {ex}");
                    }
                }

                if (loadingSceneForUnload.IsValid() && loadingSceneForUnload.isLoaded)
                {
                    yield return SceneManager.UnloadSceneAsync(loadingSceneForUnload);
                }

                yield break;
            }

            yield return null;
        }

        // 7) 新しいシーンをアクティブにする（必要なら）
        Scene newlyLoaded = SceneManager.GetSceneByName(targetSceneName);
        if (newlyLoaded.IsValid())
            SceneManager.SetActiveScene(newlyLoaded);

        string newSceneName = targetSceneName;
        if (uiManager != null)
        {
            uiManager.LoadSceneRecive(targetSceneName);
        }

        // 8) Loading シーンをアンロード（存在するか確認してから実行）
        if (loadingScene.IsValid() && loadingScene.isLoaded)
        {
            yield return SceneManager.UnloadSceneAsync(loadingScene);
        }
        else
        {
            Debug.LogWarning($"LoadingManager: Scene to unload is invalid or not loaded: {loadingSceneName}");
        }
    }
}