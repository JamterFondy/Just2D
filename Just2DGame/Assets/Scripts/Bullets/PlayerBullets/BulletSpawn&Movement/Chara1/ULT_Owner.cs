using System.Collections;
using UnityEngine;

public class ULT_Owner : MonoBehaviour
{
    [SerializeField] GameObject shooter;
    [SerializeField] GameObject chainBullet;
    [SerializeField] GameObject last_chainBullet;

    PlayerStatus playerStatus;

    public int ULTBall_Num;

    public bool isULTBall_CD;
    
    void Start()
    {
        playerStatus = GetComponent<PlayerStatus>();
        
        // スタート時の保持量は仮置きで３とする
        ULTBall_Num = 3;
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && ULTBall_Num > 0 && isULTBall_CD == false)
        {
            isULTBall_CD = true;

            StartCoroutine(ULTActivated());
        }
    }

    IEnumerator ULTActivated()
    {
        ULTBall_Num--;

        // アニメーション処理やら敵や弾の停止処理の呼び出しを行う

        StartCoroutine(SpawnULTBullet());

        yield break;
    }

    IEnumerator SpawnULTBullet()
    {
        // シューターの生成
        Vector3 currentPlayerPos = this.gameObject.transform.position;

        GameObject topShooter = Instantiate(shooter, new Vector3(currentPlayerPos.x - 0.3f, currentPlayerPos.y +1f, 0f), Quaternion.identity);
        GameObject middleShooter = Instantiate(shooter, new Vector3(currentPlayerPos.x - 0.8f, currentPlayerPos.y, 0f), Quaternion.identity);
        GameObject bottomShooter =Instantiate(shooter, new Vector3(currentPlayerPos.x - 0.3f, currentPlayerPos.y - 1f, 0f), Quaternion.identity);


        // 弾の生成


        yield break;
    }
}
