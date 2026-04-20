using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Text masterVolumeText,bgmVolumeText,seVolumeText;

    public float masterVolume, bgmVolume, seVolume;

    Slider slider;
    string sliderType; // "Master", "BGM", "SE"

    void Start()
    {
        masterVolumeText = GameObject.Find("MasterVolumeText").GetComponent<Text>();
        bgmVolumeText = GameObject.Find("BGMVolumeText").GetComponent<Text>();
        seVolumeText = GameObject.Find("SEVolumeText").GetComponent<Text>();

        slider = GetComponent<Slider>();
        if (slider == null)
        {
            Debug.LogError("Slider コンポーネントが見つかりません。");
            return;
        }

        // オブジェクト名で判定
        switch (gameObject.name)
        {
            case "MasterVolumeSlider":
                sliderType = "Master";
                masterVolume = PlayerPrefs.GetFloat("MasterVolume", 50f);
                slider.value = masterVolume;
                slider.onValueChanged.AddListener(OnMasterVolumeChanged);
                break;
            case "BGMVolumeSlider":
                sliderType = "BGM";
                bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 100f);
                slider.value = bgmVolume;
                slider.onValueChanged.AddListener(OnBGMVolumeChanged);
                break;
            case "SEVolumeSlider":
                sliderType = "SE";
                seVolume = PlayerPrefs.GetFloat("SEVolume", 100f);
                slider.value = seVolume;
                slider.onValueChanged.AddListener(OnSEVolumeChanged);
                break;
            default:
                Debug.LogWarning("VolumeSlider: 未対応のオブジェクト名です。");
                break;
        }
    }

    void OnMasterVolumeChanged(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();

        if (masterVolumeText != null)
        {
            masterVolumeText.text = $"Master Volume: {Mathf.RoundToInt(masterVolume)}%";
        }
    }

    void OnBGMVolumeChanged(float value)
    {
        bgmVolume = value;
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.Save();

        if(bgmVolumeText != null)
        {
            bgmVolumeText.text = $"BGM Volume: {Mathf.RoundToInt(bgmVolume)}%";
        }
    }

    void OnSEVolumeChanged(float value)
    {
        seVolume = value;
        PlayerPrefs.SetFloat("SEVolume", seVolume);
        PlayerPrefs.Save();

        if (seVolumeText != null)
        {
            seVolumeText.text = $"SE Volume: {Mathf.RoundToInt(seVolume)}%";
        }
    }
}