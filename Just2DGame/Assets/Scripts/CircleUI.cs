using UnityEngine;

public class CircleUI : MonoBehaviour
{
    public GameObject obj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Circle"))
        {
            obj.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        else
        {
            obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    
}
