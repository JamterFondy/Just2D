using UnityEngine;

public class Chara2NormalBulletMovement : MonoBehaviour
{
    float speed = 5f;
    float amplitude = 0.5f;
    float frequency = 2f;
    float phase = 0f;

    Vector3 startPos;
    float elapsed = 0f;

    bool initialized = false;

    public void Initialize(float spd, float amp, float freq, float ph)
    {
        speed = spd;
        amplitude = amp;
        frequency = freq;
        phase = ph;
        startPos = transform.position;
        elapsed = 0f;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        elapsed += Time.deltaTime;
        float x = startPos.x + speed * elapsed;
        float y = startPos.y + amplitude * Mathf.Sin(frequency * elapsed + phase);
        transform.position = new Vector3(x, y, startPos.z);
    }
}
