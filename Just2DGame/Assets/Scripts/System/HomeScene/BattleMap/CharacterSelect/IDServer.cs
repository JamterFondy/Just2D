using UnityEngine;

public class IDServer : MonoBehaviour
{
    public int ID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
