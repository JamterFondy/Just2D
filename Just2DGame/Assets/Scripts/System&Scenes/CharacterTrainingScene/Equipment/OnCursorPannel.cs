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
        if (IsCursorOn) return;

        if(Input.GetMouseButton(0))
        {
            GameObject pannelCanvas = transform.parent.gameObject;

            Destroy(pannelCanvas);
        }
    }
}
