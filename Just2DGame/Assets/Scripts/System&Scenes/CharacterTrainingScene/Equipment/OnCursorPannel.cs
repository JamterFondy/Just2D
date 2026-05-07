using UnityEngine;
using UnityEngine.EventSystems;

public class OnCursorPannel : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public bool IsCursorOn;

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsCursorOn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsCursorOn = false;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            GameObject pannelCanvas = transform.parent.gameObject;

            Destroy(pannelCanvas);
        }

        if (IsCursorOn) return;

        if(Input.GetMouseButton(0))
        {
            GameObject pannelCanvas = transform.parent.gameObject;

            Destroy(pannelCanvas);
        }
    }
}
