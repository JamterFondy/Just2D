using UnityEngine;
using UnityEngine.EventSystems;

public class OnCursorEquipment : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject targetClone;

    void Update()
    {
        if(targetClone != null)
        {
            if(Input.GetKey(KeyCode.Escape))
            {
                Destroy(targetClone);

                targetClone = null;
            }
        }
    }

    public void OnClick()
    {
        if(targetClone == null)
        {
            Vector3 myPos = transform.position;

            Vector3 targetPos = new Vector3(myPos.x + 1f, myPos.y - 1.5f, myPos.z - 0.5f);

            targetClone = Instantiate(target, targetPos, Quaternion.identity);
        }
        else
        {
            Destroy(targetClone);

            targetClone = null;
        }

    }

}
