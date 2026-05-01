using UnityEngine;
using UnityEngine.Audio;

public class SEVolume : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        float seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        audioSource.volume = 1.0f * masterVolume * seVolume;
    }
}
