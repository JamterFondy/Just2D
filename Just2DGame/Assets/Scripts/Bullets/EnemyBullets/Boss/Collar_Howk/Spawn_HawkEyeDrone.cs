using System.Collections;
using UnityEngine;

public class Spawn_HawkEyeDrone : MonoBehaviour
{
    [SerializeField] GameObject HawkEye_Drone;

    EnemyStatus enemyStatus;

    [Header("ドローンの生成高度の調製")] 
    [SerializeField] float drone_YPos;

    [Header("ドローンの移動設定")]
    [SerializeField] float droneMoveMaxSpeed = 5f; // maximum movement speed (units/sec)
    [SerializeField] float drone_arriveTime = 1.5f; // desired arrival time from spawn to target (seconds)

    void Start()
    {
        enemyStatus = GetComponent<EnemyStatus>();
    }

    public void SpawnDrone_Activate()
    {
        StartCoroutine(SpawnDrone());
    }

    IEnumerator SpawnDrone()
    {
        Vector2 currentHawkPos = this.gameObject.transform.position;

        GameObject UpperDrone = Instantiate(HawkEye_Drone, currentHawkPos, Quaternion.identity);
        GameObject LowerDrone = Instantiate(HawkEye_Drone, currentHawkPos, Quaternion.identity);

        Vector2 Upper_targetPos = new Vector2(0, drone_YPos);
        Vector2 Lower_targetPos = new Vector2(0, -drone_YPos);
        // start movement coroutines for both drones
        var upRoutine = StartCoroutine(MoveDroneToTarget(UpperDrone, Upper_targetPos, droneMoveMaxSpeed));
        var lowRoutine = StartCoroutine(MoveDroneToTarget(LowerDrone, Lower_targetPos, droneMoveMaxSpeed));

        // wait for both to finish
        yield return upRoutine;
        yield return lowRoutine;

        yield break;
    }

    IEnumerator MoveDroneToTarget(GameObject drone, Vector2 targetPos2D, float maxSpeed)
    {
        if (drone == null) yield break;

        // ignore z component for movement; preserve drone's z
        Vector3 startPos3 = drone.transform.position;
        Vector2 startPos2 = new Vector2(startPos3.x, startPos3.y);
        Vector3 targetPos3 = new Vector3(targetPos2D.x, targetPos2D.y, startPos3.z);

        Vector2 toTarget = targetPos2D - startPos2;
        float initialDist = toTarget.magnitude;
        if (initialDist <= 0.0001f)
        {
            // already at target
            drone.transform.position = targetPos3;
            yield break;
        }

        // compute stop threshold (1/100 of initialDist)
        float stopThreshold = Mathf.Max(initialDist / 100f, 0.001f);

        // Adjust maxSpeed so that with the proportional speed law (speed = maxSpeed * remaining / initialDist)
        // the drone effectively arrives at (or within stopThreshold of) the target in drone_arriveTime seconds.
        // For the differential equation dr/dt = -k * r with k = maxSpeed/initialDist,
        // r(t) = initialDist * exp(-k t). Solve for k so that r(T) = initialDist/100 => k = (1/T) * ln(100).
        float effectiveMaxSpeed = maxSpeed;
        if (drone_arriveTime > 0f)
        {
            float k = Mathf.Log(100f) / drone_arriveTime; // ln(100)/T
            effectiveMaxSpeed = k * initialDist; // maxSpeed = k * initialDist
        }

        float remaining = initialDist;

        while (remaining > stopThreshold)
        {
            // recompute remaining and direction each frame
            Vector3 curPos3 = drone.transform.position;
            Vector2 curPos2 = new Vector2(curPos3.x, curPos3.y);
            Vector2 delta = targetPos2D - curPos2;
            remaining = delta.magnitude;

            if (remaining <= stopThreshold) break;

            Vector2 dir = delta.normalized;

            // speed decreases proportionally as we approach the target
            // using effectiveMaxSpeed so overall arrival time approximates drone_arriveTime
            // when remaining == initialDist -> speed = effectiveMaxSpeed
            float speed = effectiveMaxSpeed * (remaining / initialDist);

            // ensure minimal movement so coroutine makes progress (but not overshoot)
            float move = speed * Time.deltaTime;
            // if move would overshoot (move > remaining), clamp to remaining
            if (move > remaining) move = remaining;

            Vector3 nextPos3 = curPos3 + new Vector3(dir.x * move, dir.y * move, 0f);
            drone.transform.position = nextPos3;

            yield return null;
        }

        // snap to exact target at the end
        if (drone != null) drone.transform.position = targetPos3;
    }
}
