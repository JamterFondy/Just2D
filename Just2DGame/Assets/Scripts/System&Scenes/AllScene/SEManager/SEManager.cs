using UnityEngine;

public class SEManager : MonoBehaviour
{
    [SerializeField] GameObject[] seObjs;
    [SerializeField] AudioClip[] seClips;

    float masterVolume;
    float seVolume;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void PlaySE(string seType, string seName, string firstName) // どんなSE(ボタン？スキル？テキスト？ => 誰の・どの音（キャンセルボタン？キャラ１？))
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        seVolume = PlayerPrefs.GetFloat("SEVolume", 1.0f);

        foreach (GameObject seObj in seObjs)
        {
            if (seObj.name == seType)
            {
                seObj.GetComponent<AudioSource>().volume = 1.0f * masterVolume * seVolume;

                foreach (AudioClip seClip in seClips)
                {
                    if(seClip.name == seName || seClip.name.StartsWith(firstName)) seObj.GetComponent<AudioSource>().clip = seClip;
                }

                if(seObj.GetComponent<AudioSource>().clip != null) seObj.GetComponent<AudioSource>().Play();

                break;
            }
        }
    }
}
