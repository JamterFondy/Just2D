using UnityEngine;

public class ButtonSE : MonoBehaviour
{
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        audioSource.volume = 1.0f * masterVolume * seVolume;
    }


    public void PlayButtonSE(string seFile)
    {
        audioSource.clip = Resources.Load<AudioClip>($"SEs/ButtonSE/{seFile}");

        audioSource.Play();
    }
}
