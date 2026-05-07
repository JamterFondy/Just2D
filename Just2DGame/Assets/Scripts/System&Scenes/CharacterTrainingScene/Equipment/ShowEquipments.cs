using UnityEngine;

public class ShowEquipments : MonoBehaviour
{
    [SerializeField] GameObject target;
    UIManager uiManager;

    Vector3 defaultPos;
    Vector3 prePos;
    Vector3 centerPos;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        uiManager = FindAnyObjectByType<UIManager>();
        if (uiManager != null)
        {
            uiManager.StateChanged += OnStateChanged;
            UpdateVisibility(uiManager.currentState);
        }
        else
        {
            Debug.LogWarning("UIManager not found. Visibility won't update automatically.");
        }

        defaultPos = transform.position; //初期の位置を獲得
    }

    void OnDestroy()
    {
        if (uiManager != null) uiManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(UIState state) => UpdateVisibility(state);

    void UpdateVisibility(UIState state)
    {
        if (target == null) return;
        target.SetActive(state == UIState.ChooseEquipment);
    }
}
