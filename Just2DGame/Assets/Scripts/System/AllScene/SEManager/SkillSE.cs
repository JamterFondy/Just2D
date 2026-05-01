using UnityEngine;

public class SkillSE : MonoBehaviour
{
    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    public void PlaySkillSE(string seFile)
    {
        audioSource.clip = Resources.Load<AudioClip>($"SEs/BattleChara1/SkillSE/{seFile}");

        audioSource.Play();
    }
}
