using UnityEngine;
public class EBulletCZakoSpeed : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 foward = transform.right;
        Vector3 left = new Vector3(-foward.y, foward.x, 0f);

        transform.position += left.normalized * speed * Time.deltaTime;
    }
}
