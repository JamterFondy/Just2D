using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnCursorEquipment : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject targetClone;

    [SerializeField] EquipInfoServer equipInfoServer;

    void Awake()
    {
        if(equipInfoServer == null) equipInfoServer = FindAnyObjectByType<EquipInfoServer>();
    }

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


            string name = gameObject.name;
            Match match = Regex.Match(name, @"\d+");

            if (match.Success)
            {
                int ID = int.Parse(match.Value);
                equipInfoServer.SaveSelectedEquipment(ID);
            }
        }
        else
        {
            Destroy(targetClone);

            targetClone = null;
        }

    }

}
