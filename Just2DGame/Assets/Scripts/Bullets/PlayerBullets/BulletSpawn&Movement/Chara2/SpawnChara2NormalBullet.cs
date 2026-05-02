using System.Collections;
using UnityEngine;

public class SpawnChara2NormalBullet : MonoBehaviour
{
    BattleESC esc;

    [Header("Spawn Settings")]
    [SerializeField] GameObject prefab;
    [SerializeField] float spawnInterval = 0.5f;
    [SerializeField] float spawnXOffset = 0.5f;
    [SerializeField] float spawnYOffset = 0.3f;

    [Header("Movement Settings")]
    [SerializeField] float speed = 7f; // speed along +x
    [SerializeField] float sinAmplitude = 0.5f;
    [SerializeField] float sinFrequency = 4f; // radians per second

    bool spawnToggle = false; // toggled by V key
    public bool canUseSkill = false; //BattleManagerによってtrueにされるのを待つ。CharacterIDに応じて使えるスキルを制限するため。

    Coroutine spawnCoroutine;

    void Start()
    {
        spawnToggle = false;

        esc = FindAnyObjectByType<BattleESC>();
    }

    void Update()
    {
        // Toggle spawning with Space key
        if (Input.GetKeyDown(KeyCode.Space) && !esc.isPaused && canUseSkill)
        {
            spawnToggle = !spawnToggle;
            if (spawnToggle)
            {
                spawnCoroutine = StartCoroutine(SpawnLoop());
            }
            else
            {
                if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
    }

    IEnumerator SpawnLoop()
    {
        if (prefab == null)
        {
            Debug.LogWarning("Chara2NormalBullet: prefab is not assigned.");
            yield break;
        }

        while (true)
        {
            Vector3 basePos = transform.position + Vector3.right * spawnXOffset;
            Vector3 topPos = basePos + Vector3.up * spawnYOffset;
            Vector3 bottomPos = basePos + Vector3.down * spawnYOffset;

            // instantiate top
            var top = Instantiate(prefab, topPos, Quaternion.identity) as GameObject;
            var moverTop = top.GetComponent<Chara2NormalBulletMovement>();
            if (moverTop == null) moverTop = top.AddComponent<Chara2NormalBulletMovement>();
            moverTop.Initialize(speed, sinAmplitude, sinFrequency, 0f);

            // instantiate bottom with phase shift PI
            var bottom = Instantiate(prefab, bottomPos, Quaternion.identity) as GameObject;
            var moverBottom = bottom.GetComponent<Chara2NormalBulletMovement>();
            if (moverBottom == null) moverBottom = bottom.AddComponent<Chara2NormalBulletMovement>();
            moverBottom.Initialize(speed, sinAmplitude, sinFrequency, Mathf.PI);

            float waited = 0f;
            while (waited < spawnInterval)
            {
                waited += Time.deltaTime;
                yield return null;
            }
        }
    }
}
