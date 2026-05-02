using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.CullingGroup;

public class BGManager : MonoBehaviour
{
    UIManager uiManager;
    public SceneType preScene; //前の状態を保存する変数。別シーンを挟んだ際に前のシーンのBGMを継続して再生するために必要。（現在は機能していない）

    public AudioSource audioSource;

    void Awake()
    {
         uiManager = FindObjectOfType<UIManager>();
         if (uiManager != null)
         {
             uiManager.SceneChanged += OnSceneChanged;
        }
         else
         {
             Debug.LogWarning("UIManager not found. BG music won't change automatically.");
         }
    }

    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = Resources.Load<AudioClip>("Audio/TitleBGM");
        audioSource.Play();

        preScene = uiManager.currentScene; //初期状態を保存
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.SceneChanged -= OnSceneChanged;
    }


    void OnSceneChanged(SceneType scene) => UpdateAudio(scene);
    void UpdateAudio(SceneType scene)
    {
        switch (scene)
        {
            case SceneType.Title:
                audioSource.clip = Resources.Load<AudioClip>("Audio/TitleBGM");
                break;
            case SceneType.Home:
                audioSource.clip = Resources.Load<AudioClip>("Audio/HomeBGM");
                break;
            case SceneType.CharacterTraining:
                audioSource.clip = Resources.Load<AudioClip>("Audio/CharaTrainingBGM");
                break;
            case SceneType.Map:
                audioSource.clip = Resources.Load<AudioClip>("Audio/StageMapBGM");
                break;
            case SceneType.Loading:
                audioSource.clip = null;
                break;
            default:
                audioSource.clip = null; // その他の状態では音楽を停止
                break;
        }
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    void Update()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1.0f);

        audioSource.volume = 1.0f * masterVolume * bgmVolume;
    }
}
