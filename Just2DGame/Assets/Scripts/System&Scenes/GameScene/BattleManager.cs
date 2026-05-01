using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    CharaInfoServer charaInfoServer;

    GameObject player;
    PlayerMovement playerMovement;

    //Character1
    GameObject chara1Bullets;
    Chara1NormalBullet chara1NormalBullet;
    ChainBullet chara1SkillBullet;


    //Character2
    GameObject chara2Bullets;
    SpawnChara2NormalBullet chara2NormalBullet;
    SpawnFishBullet chara2SkillBullet;



    int CharacterID;

    void Awake()
    {
        charaInfoServer = FindObjectOfType<CharaInfoServer>();
        CharacterID = charaInfoServer.ID;

        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(charaInfoServer == null)
        {
            Debug.Log("CharaInfoServer is NULL at BattleManager");
        }


        if (CharacterID == 1)
        {
            chara1Bullets = GameObject.Find("Chara1Bullets");
            chara1NormalBullet = chara1Bullets.GetComponent<Chara1NormalBullet>();
            chara1SkillBullet = chara1Bullets.GetComponent<ChainBullet>();
        }
        else if(CharacterID == 2)
        {
            chara2Bullets = GameObject.Find("Chara2Bullets");
            chara2NormalBullet = chara2Bullets.GetComponent<SpawnChara2NormalBullet>();
            chara2SkillBullet = chara2Bullets.GetComponent<SpawnFishBullet>();
        }





        StartCoroutine(WaitStartBattle());
    }


    IEnumerator WaitStartBattle()
    {
        yield return new WaitForSeconds(5f); // バトル開始の演出が終わるのを待つ。その間は弾は使えない。

        if (CharacterID == 1)
        {
            chara1NormalBullet.canUseSkill = true;
            chara1SkillBullet.canUseSkill = true;
        }
        else if (CharacterID == 2)
        {
            chara2NormalBullet.canUseSkill = true;
            chara2SkillBullet.canUseSkill = true;
        }


        playerMovement.CanMove = true; // プレイヤーの移動を許可

        yield break;
    }
}
