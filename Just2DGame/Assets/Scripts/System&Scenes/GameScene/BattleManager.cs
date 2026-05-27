using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BattleManager : MonoBehaviour
{
    PlayerStatus playerStatus;
    CharaInfoServer charaInfoServer;

    GameObject player;
    PlayerMovement playerMovement;

    [SerializeField] GameObject stageQuestPannel;



    int CharacterID;

    void Awake()
    {
        playerStatus = FindAnyObjectByType<PlayerStatus>();
        charaInfoServer = FindAnyObjectByType<CharaInfoServer>();
        CharacterID = charaInfoServer.ID;

        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

    }

    void Start()
    {
        if(playerStatus == null)
        {
            Debug.Log("PlayerStatus is NULL at BattleManager");
        }
        else
        {
            playerStatus.currentControlState = PlayerControlState.BattleStart; // 開始時点でPlayerのスキル使用や動きを封じる。
        }

        if(charaInfoServer == null)
        {
            Debug.Log("CharaInfoServer is NULL at BattleManager");
        }


        StartCoroutine(WaitStartBattle());
        StartCoroutine(MoveQuestPannelRight());
    }


    IEnumerator WaitStartBattle()
    {
        yield return new WaitForSeconds(5f); // バトル開始の演出が終わるのを待つ。その間は弾は使えない。

        playerStatus.currentControlState = PlayerControlState.None;

        playerMovement.CanMove = true; // プレイヤーの移動を許可

        yield break;
    }

    IEnumerator MoveQuestPannelRight()
    {      
        yield return new WaitForSeconds(2f); 
        stageQuestPannel.SetActive(true);

        yield return new WaitForSeconds(2f); 
        stageQuestPannel.gameObject.SetActive(false);
    }
}
