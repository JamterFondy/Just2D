using UnityEngine;
using System.Collections;

public class Chara1NormalBullet : MonoBehaviour
{
    PlayerStatus playerStatus;

    [SerializeField] GameObject normalBullet; // 通常弾
    [SerializeField] BattleESC esc;

    [SerializeField] float bulletSpeed = 30f; // 弾の移動速度（Inspectorで設定可能）

    public bool SpaceToggle = false; // スペースキーでのトグルフラグ

    public bool canUseSkill = false; // BattleManagerによってtrueにされるのを待つ。CharacterIDに応じて使えるスキルを制限するため。
                                     // バトル終了時にPlayerStatusの状態を受けてfalseになる。


    void Awake()
    {
        canUseSkill = false;

        playerStatus = FindAnyObjectByType<PlayerStatus>();
        if (playerStatus != null)
        {
            playerStatus.PlayerControlStateChanged += OnPlayerControlStateChanged;
        }
        else
        {
            Debug.LogWarning("PlayerStatus not found. Skill usage will not be properly restricted.");

        }

    }
   
    void Start()
    {
        esc = FindAnyObjectByType<BattleESC>();

        StartCoroutine(SpawnNormalBullet());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !esc.isPaused && canUseSkill)
        {
            SpaceToggle = !SpaceToggle;
        }
    }

    void OnPlayerControlStateChanged(PlayerControlState playerControlState) => CanUseChanged(playerControlState);


    void CanUseChanged(PlayerControlState state)
    {
        
        if (state == PlayerControlState.BattleStart || state == PlayerControlState.BattleFinished)
        {
            canUseSkill = false;
            SpaceToggle = false; // スキル使用不可の状態になったらスペーストグルもリセット。
        }
        else
        {
            canUseSkill = true;
        }
        

    }

    IEnumerator SpawnNormalBullet()
    {

        Quaternion angler = Quaternion.Euler(0f, 0f, 90f);

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            // SpaceToggle が有効なときだけ弾を生成する
            if (!SpaceToggle)
                continue;

            Vector3 pos = transform.position;

            // Instantiate and apply movement to the right (+x) when SpaceToggle is true
            var b1 = Instantiate(normalBullet, new Vector3(pos.x - 2f, pos.y, pos.z + 0.5f), angler) as GameObject;
            var b2 = Instantiate(normalBullet, new Vector3(pos.x - 2f, pos.y + 0.2f, pos.z + 0.5f), angler) as GameObject;
            var b3 = Instantiate(normalBullet, new Vector3(pos.x - 2f, pos.y - 0.2f, pos.z + 0.5f), angler) as GameObject;

            ApplyRightwardMovement(b1);
            ApplyRightwardMovement(b2);
            ApplyRightwardMovement(b3);

        }
    }

    void ApplyRightwardMovement(GameObject bullet)
    {
            if (bullet == null) return;

            // Try Rigidbody2D first
            var rb2d = bullet.GetComponent<Rigidbody2D>();
            if (rb2d != null)
            {
                rb2d.linearVelocity = new Vector2(bulletSpeed, 0f);
                return;
            }

            // Try 3D Rigidbody
            var rb3d = bullet.GetComponent<Rigidbody>();
            if (rb3d != null)
            {
                rb3d.linearVelocity = new Vector3(bulletSpeed, 0f, 0f);
                return;
            }

            // Fallback: add a simple mover component
            var mover = bullet.GetComponent<SimpleBulletMover>();
            if (mover == null) mover = bullet.AddComponent<SimpleBulletMover>();
            mover.Initialize(bulletSpeed, Vector3.right);
    }

}
    

