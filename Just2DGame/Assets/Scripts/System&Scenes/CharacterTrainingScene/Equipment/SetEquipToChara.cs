using UnityEngine;

public class SetEquipToChara : MonoBehaviour
{
    public void OnClick()
    {
        GameObject Pannel = transform.root.gameObject;

        Destroy(Pannel);
    }
}
