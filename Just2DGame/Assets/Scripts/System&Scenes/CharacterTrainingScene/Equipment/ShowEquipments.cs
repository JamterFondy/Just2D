using UnityEngine;

public class ShowEquipments : MonoBehaviour
{
    [SerializeField] GameObject target;

    Vector3 defaultPos;
    Vector3 prePos;
    Vector3 centerPos;

    void Awake()
    {
        if (target == null) target = this.gameObject;
       
        UIManager.Instance.StateChanged += OnStateChanged;
        UpdateVisibility(UIManager.Instance.currentState);
        

        defaultPos = transform.position; //初期の位置を獲得
    }

    void OnDestroy()
    {
        UIManager.Instance.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.ChooseEquipment);
    }
}
