using UnityEngine;
using UnityEngine.EventSystems;

public class Character2 : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] SelectCharacter selectCharacter;

    int storedOriginalID;
    bool hasStoredOriginal;
    bool isPointerOver;
    bool isPointerDown;

    void Start()
    {
        // Inspector に未割り当てなら近くの SelectCharacter を自動で探す
        if (selectCharacter == null)
        {
            selectCharacter = GetComponentInParent<SelectCharacter>();
            if (selectCharacter == null)
            {
                selectCharacter = FindObjectOfType<SelectCharacter>();
            }
        }
    }

    void Update()
    {
        if(selectCharacter.characterID != 2)
        {
            isPointerDown = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        storedOriginalID = selectCharacter.characterID;

        isPointerOver = true;
        TrySetToTwo();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        TrySetToTwo();
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        // カーソルがボタン上にないか、または現在 characterID が 2 でないなら復元
        if (!isPointerOver || (selectCharacter != null && selectCharacter.characterID != 2))
        {
            RestoreOriginal();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        // 押されていない状態なら復元（または外部で characterID が変わっていれば Update 側で復元される）
        if (!isPointerDown || (selectCharacter != null && selectCharacter.characterID != 2))
        {
            RestoreOriginal();
        }
    }

    void TrySetToTwo()
    {
        if (selectCharacter == null) return;

        selectCharacter.characterID = 2;
    }

    void RestoreOriginal()
    {
        if (selectCharacter == null) return;

        selectCharacter.characterID = storedOriginalID;
    }
}