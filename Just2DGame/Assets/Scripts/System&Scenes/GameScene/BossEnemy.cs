using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    public GameObject bossHPBar; // ステージローダーから割り当てられる
    [SerializeField] EnemyStatus enemyStatus;
    BattleFinish battleFinish;
    [SerializeField] Transform hpBarImage;

    int maxHP;

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
        battleFinish = FindAnyObjectByType<BattleFinish>();

        maxHP = enemyStatus.hp;

        var hpBarInstance = Instantiate(bossHPBar);

        hpBarImage = hpBarInstance.transform.Find("BossHPBarCanvas/BossHPBarImage");

        StartCoroutine(FillHPBar());
    }
  
    void Update()
    {
        if (maxHP != 0)
        {
            float hpRatio = (float)enemyStatus.hp / maxHP;
            hpBarImage.GetComponent<Image>().fillAmount = hpRatio;
        }
    }

    IEnumerator FillHPBar()
    {
        float currentFillAmount = hpBarImage.GetComponent<Image>().fillAmount;

        yield return null;
    }

    public void BossDied()
    {
        if (bossHPBar != null)
        {
            hpBarImage.GetComponent<Image>().fillAmount = 0f;
        }
        maxHP = 0;


        battleFinish.CountBossDied();
    }
}
