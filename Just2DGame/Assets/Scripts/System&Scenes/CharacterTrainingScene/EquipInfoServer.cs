using UnityEngine;

public class EquipInfoServer : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveSelectedEquipment(string name)
    {
        PlayerPrefs.SetString("SelectedEquipmentName", name);
        PlayerPrefs.Save();
    }
}
