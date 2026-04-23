using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.CullingGroup;

public class BGManager : MonoBehaviour
{
    UIManager uiManager;
    public UIState preState; //前の状態を保存する変数。Settingsを挟んだ際に前のシーンのBGMを継続して再生するために必要。

    public AudioSource audioSource;

    void Awake()
    {
         uiManager = FindObjectOfType<UIManager>();
         if (uiManager != null)
         {
             uiManager.StateChanged += OnStateChanged;
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

        preState = uiManager.currentState; //初期状態を保存
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.StateChanged -= OnStateChanged;
    }


    void OnStateChanged(UIState state) => UpdateAudio(state);

    void UpdateAudio(UIState state)
    {
        if (state == UIState.Settings) return; //設定を開く前後でBGMを再度読み込まない。
        if (audioSource == null) return;

        //UIStateがSettings以外で他の状態に変わった際に今回のシーンを保存。
        //Settingsを挟んだ際に前のシーンのBGMを継続して再生するために必要な処理。
        if (preState == state) return;
        preState = state; 
        

        switch (state)
        {
            case UIState.TitleDefault:
                audioSource.clip = Resources.Load<AudioClip>("Audio/TitleBGM");
                break;
            case UIState.HomeDefault:
                audioSource.clip = Resources.Load<AudioClip>("Audio/HomeBGM");
                break;
            case UIState.CharaTrainingDefault:
                audioSource.clip = Resources.Load<AudioClip>("Audio/CharaTrainingBGM");
                break;
            case UIState.StageMapDefault:
                audioSource.clip = Resources.Load<AudioClip>("Audio/StageMapBGM");
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
