using UnityEngine;

public class FishBulletMovement : MonoBehaviour
{
    // Movement parameters
    float moveDuration = 0.9f;
    float straightSpeed = 5f;
    float ellipseYScale = 0.5f; // b = a * ellipseYScale
    // internal state
    bool running = false;

    [Tooltip("Rotation smoothing speed in degrees per second")]
    public float rotationSmoothSpeed = 720f;

    public void Initialize(float duration, float straightSpd, float yScale = 0.5f)
    {
        moveDuration = duration;
        straightSpeed = straightSpd;
        ellipseYScale = yScale;
    }

    // Start the elliptical arc from current position to target
    // startPos is transform.position when called; target in world space
    // upperArc: true => traverse upper half (y positive), false => traverse lower half (y negative)
    public void StartEllipseToTarget(Vector3 target, bool upperArc = true)
    {
        if (running) return;
        StartCoroutine(MoveEllipseCoroutine(transform.position, target, upperArc));
    }

    System.Collections.IEnumerator MoveEllipseCoroutine(Vector3 startPos, Vector3 target, bool upperArc)
    {
        running = true;

        // compute local x-axis from start to target
        Vector3 delta = target - startPos;
        float dist = delta.magnitude;
        if (dist < 0.001f)
        {
            // fallback: move straight
            running = false;
            yield break;
        }

        // ellipse parameters: center at midpoint, semi-major a = dist/2, semi-minor b = a * ellipseYScale
        float a = dist * 0.5f;
        float b = a * ellipseYScale; // ellipseYScale controls y amplitude relative to x (b = a * scale)

        Vector3 center = startPos + delta * 0.5f;
        // base rotation that aligns local ellipse x-axis with world delta
        float baseAngle = Mathf.Atan2(delta.y, delta.x);
        Quaternion baseRot = Quaternion.Euler(0f, 0f, baseAngle * Mathf.Rad2Deg);

        // param theta: for upperArc go from PI -> 0; for lowerArc go from -PI -> 0 over moveDuration
        float elapsed = 0f;
        float lastTangentAngle = 0f;

        // previousWorldPos used to compute actual motion direction (velocity) so rotation aligns with movement
        Vector3 previousWorldPos = startPos;

        while (elapsed < moveDuration)
        {
            float t = Mathf.Clamp01(elapsed / moveDuration);
            float thetaStart = upperArc ? Mathf.PI : -Mathf.PI;
            float theta = Mathf.Lerp(thetaStart, 0f, t);

            // local ellipse coordinates
            float lx = a * Mathf.Cos(theta);
            float ly = b * Mathf.Sin(theta);

            Vector3 localPos = new Vector3(lx, ly, 0f);
            Vector3 worldPos = center + baseRot * localPos;

            // compute motion-based tangent (world velocity direction)
            Vector3 motionDelta = worldPos - previousWorldPos;
            float tangentAngle;
            if (motionDelta.sqrMagnitude > 1e-6f)
            {
                tangentAngle = Mathf.Atan2(motionDelta.y, motionDelta.x) * Mathf.Rad2Deg;
            }
            else
            {
                // fallback to analytic tangent if motion delta too small
                float dx = -a * Mathf.Sin(theta);
                float dy = b * Mathf.Cos(theta);
                Vector3 localTangent = new Vector3(dx, dy, 0f);
                Vector3 worldTangent = baseRot * localTangent;
                tangentAngle = Mathf.Atan2(worldTangent.y, worldTangent.x) * Mathf.Rad2Deg;
            }

            lastTangentAngle = tangentAngle;

            // place and rotate so that object's +x aligns with motion direction
            transform.position = worldPos;
            var targetRot = Quaternion.Euler(0f, 0f, tangentAngle);
            // smooth rotation to avoid instant flips
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSmoothSpeed * Time.deltaTime);

            previousWorldPos = worldPos;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // ensure at exact target
        transform.position = target;
        // keep last tangent angle as forward direction
        transform.rotation = Quaternion.Euler(0f, 0f, lastTangentAngle);

        // now move straight along transform.right (local +x) indefinitely
        while (true)
        {
            transform.position += transform.right * straightSpeed * Time.deltaTime;
            yield return null;
        }
    }
}
