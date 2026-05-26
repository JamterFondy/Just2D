using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    [SerializeField] LogInOutManager logInOutManager;
    
    void Awake()
    {
        if (PlayerPrefs.HasKey("ScrapNum") == false)
        {
            PlayerPrefs.SetInt("ScrapNum", 0);
        }
        
        if(PlayerPrefs.HasKey("BoneCoin") == false)
        {
            PlayerPrefs.SetInt("BoneCoin", 0);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        logInOutManager = FindAnyObjectByType<LogInOutManager>();

        OfflineIncome();
    }

    void OfflineIncome()
    {
        int scrapNum = PlayerPrefs.GetInt("ScrapNum");
        int boneCoin = PlayerPrefs.GetInt("BoneCoin");
        int offlineIncome = logInOutManager.minutesOfLogout * 10;

        // ここらへんもバーの収入とか装飾とかレベルの進み具合で増加量を変えたい
        scrapNum += offlineIncome;
        boneCoin += offlineIncome / 10;

        PlayerPrefs.SetInt("ScrapNum", scrapNum);
        PlayerPrefs.SetInt("BoneCoin", boneCoin);
        Debug.Log($"オフライン収入: {offlineIncome}スクラップ, {offlineIncome / 10}ボーンコイン");
    }


}
