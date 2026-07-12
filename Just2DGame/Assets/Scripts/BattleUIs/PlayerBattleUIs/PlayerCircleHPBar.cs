using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PlayerCircleHPBar : MonoBehaviour
{
    [SerializeField] float circleHPRadius = 1f;
    // fraction for inner radius (inner = radius - radius * innerFraction)
    [SerializeField, Range(0f, 0.9f)] float innerFraction = 0.25f;

    // full arc in degrees (from 12 o'clock to 3 o'clock -> 90 deg)
    const float fullArcDegrees = 90f;
    // start angle in degrees (12 o'clock)
    const float startAngleDeg = 90f;

    // segments per degree approx
    [SerializeField] int segmentsPer90 = 30; // approx segments for full 90deg arc

    // reference to player status
    PlayerStatus playerStatus;

    // Max HP captured initially
    int maxHP = 1;

    // cached mesh
    Mesh mesh;
    float lastRatio = -1f;

    [Header("2D Rendering")]
    [SerializeField] bool use2D = true;
    [SerializeField] string sortingLayerName = "Default";
    [SerializeField] int sortingOrder = 0;
    // Optional: assign a material that uses a sprite/unlit shader to render correctly in 2D
    [SerializeField] Material overrideMaterial;

    // If you want to assign a material via inspector, uncomment below and assign
    // [SerializeField] Material barMaterial; // assign a simple unlit material for the bar

    void Start()
    {
        // try to find PlayerStatus in scene
        playerStatus = FindObjectOfType<PlayerStatus>();
        if (playerStatus == null)
        {
            Debug.LogWarning("PlayerCircleHPBar: PlayerStatus not found. HP bar will not update.");
            maxHP = 1;
        }
        else
        {
            // capture initial HP as MaxHP
            maxHP = Mathf.Max(1, playerStatus.hp);
        }

        var mf = GetComponent<MeshFilter>();
        if (mf.sharedMesh == null)
        {
            mesh = new Mesh();
            mesh.name = "HPBarMesh";
            mf.sharedMesh = mesh;
        }
        else
        {
            mesh = mf.sharedMesh;
        }

        // Configure MeshRenderer for 2D rendering
        var mr = GetComponent<MeshRenderer>();
        if (mr != null && use2D)
        {
            // ensure a sprite-compatible shader/material so material renders with transparency in 2D
            if (overrideMaterial != null)
            {
                mr.sharedMaterial = overrideMaterial;
            }
            else
            {
                var spriteShader = Shader.Find("Sprites/Default");
                if (spriteShader != null)
                    mr.sharedMaterial = new Material(spriteShader);
            }

            // set sorting so it participates in 2D draw order
            mr.sortingLayerName = sortingLayerName;
            mr.sortingOrder = sortingOrder;
        }

        // initial mesh build
        UpdateMeshForRatio(1f);
    }

    void Update()
    {
        if (playerStatus == null) return;

        float ratio = Mathf.Clamp01((float)playerStatus.hp / (float)Mathf.Max(1, maxHP));
        if (Mathf.Approximately(ratio, lastRatio)) return;
        lastRatio = ratio;
        UpdateMeshForRatio(ratio);
    }

    void UpdateMeshForRatio(float ratio)
    {
        mesh.Clear();

        // clamp ratio
        ratio = Mathf.Clamp01(ratio);
        if (ratio <= 0f)
        {
            // nothing visible
            return;
        }

        float arcDeg = fullArcDegrees * ratio; // visible arc length

        // determine number of segments
        int segments = Mathf.Max(2, Mathf.RoundToInt((segmentsPer90 * arcDeg) / fullArcDegrees));

        float innerR = circleHPRadius - circleHPRadius  * innerFraction;

        // vertices: outer 0..segments, inner segments+1..2*segments+1
        Vector3[] verts = new Vector3[(segments + 1) * 2];
        int vertIndex = 0;

        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / (float)segments; // 0..1
            float angle = startAngleDeg - arcDeg * t; // clockwise from 12 o'clock
            float rad = Mathf.Deg2Rad * angle;
            Vector3 pOuter = new Vector3(Mathf.Cos(rad) * circleHPRadius, Mathf.Sin(rad) * circleHPRadius, 0f);
            verts[vertIndex++] = pOuter;
        }
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / (float)segments;
            float angle = startAngleDeg - arcDeg * t;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 pInner = new Vector3(Mathf.Cos(rad) * innerR, Mathf.Sin(rad) * innerR, 0f);
            verts[vertIndex++] = pInner;
        }

        // triangles
        int quadCount = segments;
        int[] tris = new int[quadCount * 6];
        int ti = 0;
        for (int i = 0; i < segments; i++)
        {
            int o0 = i;
            int o1 = i + 1;
            int i0 = (segments + 1) + i;
            int i1 = (segments + 1) + i + 1;

            // triangle 1: o0, o1, i1
            tris[ti++] = o0;
            tris[ti++] = o1;
            tris[ti++] = i1;
            // triangle 2: o0, i1, i0
            tris[ti++] = o0;
            tris[ti++] = i1;
            tris[ti++] = i0;
        }

        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }

    // Optional public method to set MaxHP explicitly (call from PlayerStatus if desired)
    // public void SetMaxHP(int hp) { maxHP = Mathf.Max(1, hp); }
}
