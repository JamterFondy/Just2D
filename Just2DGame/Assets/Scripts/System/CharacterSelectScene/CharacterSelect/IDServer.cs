using UnityEngine;

public class IDServer : MonoBehaviour
{
    [SerializeField] GameObject startBattle;

    public int ID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
