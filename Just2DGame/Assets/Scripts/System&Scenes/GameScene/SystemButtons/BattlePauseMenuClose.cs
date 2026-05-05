using System.Collections;
using UnityEngine;

public class BattlePauseMenuClose : MonoBehaviour
{
    BattleESC battleESC;


    // Update is called once per frame
    void Update()
    {     
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick();
        }
        
    }

    public void OnClick()
    {
        battleESC = FindAnyObjectByType<BattleESC>();

        battleESC.RestartBattle();
    }

}
