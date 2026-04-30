using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnFishBullet : MonoBehaviour
{
    [SerializeField] Sprite chargingSprite; // クールタイム中のスプライト
    [SerializeField] Sprite fullSprite;// ゲージ満タンのスプライト

    Camera cam;
    [SerializeField] SkillGage skillGage;
    PlayerStatus playerStatus;

    [SerializeField] BattleESC esc;

    [SerializeField] GameObject prefab1;     // 上下に生成するオブジェクト（1つずつ）
    [SerializeField] GameObject prefab2;     // １秒間隔で上下に生成される弾

    [SerializeField] float verticalOffset = 0.8f; // prefab1 の上下オフセット
    [SerializeField] float leftCrickCoolTime = 4f; // 左クリックのクールタイム
    [SerializeField] float spawnInterval = 0.5f; // interval between spawns (seconds)
    [SerializeField] int repeatCount = 6; // number of times to spawn prefab2
    [SerializeField] float prefab2MoveDuration = 0.9f; // duration to follow ellipse
    [SerializeField] float prefab2StraightSpeed = 5f; // speed after reaching target

    public bool LeftCrickCoolTime = false;

    public bool canUseSkill = false; //BattleManagerによってtrueにされるのを待つ。CharacterIDに応じて使えるスキルを制限するため。

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        canUseSkill = false;
    }

    void Start()
    {
        if (playerStatus == null)
        {
            playerStatus = FindObjectOfType<PlayerStatus>();
        }

        playerStatus.leftCrickCT = leftCrickCoolTime;


        if (esc == null)
        {
            esc = FindObjectOfType<BattleESC>();
        }

        cam = Camera.main;
        if (cam == null)
            Debug.LogWarning("Main Camera not found. Movement bounds will not be applied.");

        if (skillGage == null)
        {
            skillGage = FindObjectOfType<SkillGage>().GetComponent<SkillGage>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!LeftCrickCoolTime && Input.GetMouseButtonDown(0) && !esc.isPaused && canUseSkill)
        {
            LeftCrickCoolTime = true;


            Vector3 mouseScreen = Input.mousePosition;
            mouseScreen.z = transform.position.z - cam.transform.position.z;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(mouseScreen);
            mouseWorld.z = transform.position.z;

            Vector3 myPos = transform.position;

            Vector3 topPos = new Vector3(myPos.x, myPos.y + verticalOffset, myPos.z);
            Vector3 bottomPos = new Vector3(myPos.x, myPos.y - verticalOffset, myPos.z);

            GameObject topObj = null;
            GameObject bottomObj = null;

            if (prefab1 != null)
            {
                topObj = Instantiate(prefab1, topPos, Quaternion.identity) as GameObject;
                bottomObj = Instantiate(prefab1, bottomPos, Quaternion.identity) as GameObject;

                // start spawn sequence coroutine which will spawn prefab2 instances
                StartCoroutine(SpawnSequence(topObj, bottomObj));
            }
            else
            {
                Debug.LogWarning("prefab1 is not assigned.");
            }


        }
    }

    IEnumerator SpawnSequence(GameObject topObj, GameObject bottomObj)
    {
        if (topObj == null || bottomObj == null)
            yield break;

        for (int i = 0; i < repeatCount; i++)
        {
            // spawn prefab2 at top and bottom positions
            Vector3 topPos = topObj.transform.position;
            Vector3 bottomPos = bottomObj.transform.position;

            if (prefab2 != null)
            {
                // sample mouse position at spawn time for this iteration
                Vector3 mouseScreen = Input.mousePosition;
                float refZ = transform.position.z;
                if (playerStatus != null) refZ = playerStatus.transform.position.z;
                if (cam == null) cam = Camera.main;
                if (cam == null)
                {
                    Debug.LogWarning("SpawnFishBullet: Camera.main not found when sampling mouse position.");
                }
                mouseScreen.z = refZ - (cam != null ? cam.transform.position.z : 0f);
                Vector3 mouseWorld = cam != null ? cam.ScreenToWorldPoint(mouseScreen) : Vector3.zero;
                mouseWorld.z = refZ;

                bool spawnTopThisIteration = (i % 2 == 0); // alternate: first top, then bottom, ...

                if (spawnTopThisIteration)
                {
                    // spawn three variants for top (default 2:1, 1:1, 3:1)
                    var bTop_default = Instantiate(prefab2, topPos, Quaternion.identity) as GameObject;
                    var bTop_1to1 = Instantiate(prefab2, topPos, Quaternion.identity) as GameObject;
                    var bTop_3to1 = Instantiate(prefab2, topPos, Quaternion.identity) as GameObject;

                    // Initialize and start movement for top variants
                    var mvTop_def = bTop_default.GetComponent<FishBulletMovement>();
                    if (mvTop_def == null) mvTop_def = bTop_default.AddComponent<FishBulletMovement>();
                    mvTop_def.Initialize(prefab2MoveDuration, prefab2StraightSpeed, 0.5f); // x:y = 2:1
                    mvTop_def.StartEllipseToTarget(mouseWorld, true);

                    var mvTop_11 = bTop_1to1.GetComponent<FishBulletMovement>();
                    if (mvTop_11 == null) mvTop_11 = bTop_1to1.AddComponent<FishBulletMovement>();
                    mvTop_11.Initialize(prefab2MoveDuration, prefab2StraightSpeed, 1f); // x:y = 1:1
                    mvTop_11.StartEllipseToTarget(mouseWorld, true);

                    var mvTop_31 = bTop_3to1.GetComponent<FishBulletMovement>();
                    if (mvTop_31 == null) mvTop_31 = bTop_3to1.AddComponent<FishBulletMovement>();
                    mvTop_31.Initialize(prefab2MoveDuration, prefab2StraightSpeed, 1f/3f); // x:y = 3:1
                    mvTop_31.StartEllipseToTarget(mouseWorld, true);
                }
                else
                {
                    // spawn three variants for bottom
                    var bBottom_default = Instantiate(prefab2, bottomPos, Quaternion.identity) as GameObject;
                    var bBottom_1to1 = Instantiate(prefab2, bottomPos, Quaternion.identity) as GameObject;
                    var bBottom_3to1 = Instantiate(prefab2, bottomPos, Quaternion.identity) as GameObject;

                    // Initialize and start movement for bottom variants
                    var mvBottom_def = bBottom_default.GetComponent<FishBulletMovement>();
                    if (mvBottom_def == null) mvBottom_def = bBottom_default.AddComponent<FishBulletMovement>();
                    mvBottom_def.Initialize(prefab2MoveDuration, prefab2StraightSpeed, 0.5f);
                    mvBottom_def.StartEllipseToTarget(mouseWorld, false);

                    var mvBottom_11 = bBottom_1to1.GetComponent<FishBulletMovement>();
                    if (mvBottom_11 == null) mvBottom_11 = bBottom_1to1.AddComponent<FishBulletMovement>();
                    mvBottom_11.Initialize(prefab2MoveDuration, prefab2StraightSpeed, 1f);
                    mvBottom_11.StartEllipseToTarget(mouseWorld, false);

                    var mvBottom_31 = bBottom_3to1.GetComponent<FishBulletMovement>();
                    if (mvBottom_31 == null) mvBottom_31 = bBottom_3to1.AddComponent<FishBulletMovement>();
                    mvBottom_31.Initialize(prefab2MoveDuration, prefab2StraightSpeed, 1f/3f);
                    mvBottom_31.StartEllipseToTarget(mouseWorld, false);
                }
            }
            else
            {
                Debug.LogWarning("prefab2 is not assigned.");
            }

            // wait interval before next spawn
            float waited = 0f;
            while (waited < spawnInterval)
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }

        // after repeats, destroy prefab1 objects
        if (topObj != null) Destroy(topObj);
        if (bottomObj != null) Destroy(bottomObj);

        
        StartCoroutine(CoolTimeL(leftCrickCoolTime));

    }


    IEnumerator CoolTimeL(float cooltime)
    {
        playerStatus.LeftCrickCTBool = true;　//クールタイムが正式に開始（Charging状態）
        if (skillGage != null && chargingSprite != null)
        {
            skillGage.SetCoolTimeImage(chargingSprite, leftCrickCoolTime);
        }

        yield return new WaitForSeconds(cooltime);

        LeftCrickCoolTime = false;
        playerStatus.LeftCrickCTBool = false;
        if (skillGage != null && fullSprite != null)
        {
            skillGage.SetFullImage(fullSprite);
        }

    }
}
