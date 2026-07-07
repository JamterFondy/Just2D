using UnityEngine;
using System.Collections;

public class ShooterDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
               
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }
}
