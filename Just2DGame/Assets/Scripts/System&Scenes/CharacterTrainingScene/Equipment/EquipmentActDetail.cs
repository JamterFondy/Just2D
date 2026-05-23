using UnityEngine;

public class EquipmentActDetail : MonoBehaviour
{
    EquipCharaChooseUIs equipCharaChooseUIs;

    GameObject equipToCharaUI;

    string equipmentName;
    void Awake()
    {
        equipToCharaUI = GameObject.Find("SetEquipToCharaUIs");
        equipCharaChooseUIs = equipToCharaUI.GetComponent<EquipCharaChooseUIs>();

    }
    public void ShowEquipToCharaPannel()
    {
        equipmentName = PlayerPrefs.GetString("SelectedEquipmentName");
        equipCharaChooseUIs.ReceiveData(equipmentName, 0);
    }

    public void LevelUpEquipPannel()
    {
       // equipToCharaPannel.SetActive(false);
    }
}
