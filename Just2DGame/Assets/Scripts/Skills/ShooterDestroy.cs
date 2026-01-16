using UnityEngine;
using System.Collections;

public class ShooterDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Destroy()
    {
               
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }
}
