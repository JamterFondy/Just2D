using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] GameObject bossHPBar;
    [SerializeField] EnemyStatus enemyStatus;
    BattleFinish battleFinish;
    Transform hpBarImage;

    int maxHP;

    void Start()
    {
        enemyStatus =  gameObject.GetComponent<EnemyStatus>();
        battleFinish = FindAnyObjectByType<BattleFinish>();

        maxHP = enemyStatus.hp;
        Instantiate(bossHPBar);
        hpBarImage = bossHPBar.transform.Find("BossHPBarImg");

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

        while (currentFillAmount < 1f)
        {
            hpBarImage.GetComponent<Image>().fillAmount += 0.01f;
        }

        yield return null;
    }

    public void BossDied()
    {
        if (bossHPBar != null)
        {
            Destroy(bossHPBar);
            hpBarImage.GetComponent<Image>().fillAmount = 0f;
        }
        maxHP = 0;


        battleFinish.CountBossDied();
    }
}
