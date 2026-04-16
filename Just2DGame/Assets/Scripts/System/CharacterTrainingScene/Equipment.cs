using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;

public class Equipment : MonoBehaviour
{
    [SerializeField] GameObject target; // 表示/非表示を切り替える対象（ボタン本体など）
    [SerializeField] CharaTrainingUIManager charaTrainingUIManager;

    void Awake()
    {
        if (target == null) target = this.gameObject;
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();
        if (charaTrainingUIManager != null)
        {
            charaTrainingUIManager.StateChanged += OnStateChanged;
            UpdateVisibility(charaTrainingUIManager.currentState);
        }
        else
        {
            Debug.LogWarning("CharaTrainingUIManager not found. Visibility won't update automatically.");
        }
    }
    void Start()
    {
        charaTrainingUIManager = FindObjectOfType<CharaTrainingUIManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        if (charaTrainingUIManager != null) charaTrainingUIManager.StateChanged -= OnStateChanged;
    }

    void OnStateChanged(CharaTrainingUIState state) => UpdateVisibility(state);

    void UpdateVisibility(CharaTrainingUIState state)
    {
        if (target == null) return;
        target.SetActive(state == CharaTrainingUIState.None);
    }

    public void OnClick()
    {
        charaTrainingUIManager.currentState = CharaTrainingUIState.Equipment;
    }
}
