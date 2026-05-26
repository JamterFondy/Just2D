using System;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LogInOutManager : MonoBehaviour
{

    public int minutesOfLogout;

    void Awake()
    {
        if (PlayerPrefs.HasKey("LastLogoutTime"))
        {
            DateTime lastLogIn = DateTime.Parse(PlayerPrefs.GetString("LastLogoutTime"));
            DateTime now = DateTime.Now;

            TimeSpan timeSpan = now - lastLogIn;

            minutesOfLogout = (int)timeSpan.TotalMinutes;
            Debug.Log($"前回のログアウトからの経過時間: {timeSpan.TotalMinutes}分");
        }
    }
    

    void OnApplicationQuit()
    {
        SaveLogoutTime();
    }

   void OnEnable()
   {
#if UNITY_EDITOR
       EditorApplication.playModeStateChanged += OnPlayModeChanged;
#endif
   }

   void OnDisable()
   {
#if UNITY_EDITOR
       EditorApplication.playModeStateChanged -= OnPlayModeChanged;
#endif
   }

#if UNITY_EDITOR
   void OnPlayModeChanged(PlayModeStateChange state)
   {
       if (state == PlayModeStateChange.ExitingPlayMode)
       {
        SaveLogoutTime();
        Debug.Log("【Editor】Playモード終了 → ログアウト時間を保存");
       }
   }
#endif


   void SaveLogoutTime()
   {
       PlayerPrefs.SetString("LastLogoutTime", DateTime.Now.ToString("o"));
       PlayerPrefs.Save();
   }


}
