using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public Transform itemListParent;
    public GameObject itemSlotPrefab;

    private List<GameObject> itemSlots = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RefreshUI()
    {
        // 기존 슬롯 제거
        foreach (var slot in itemSlots) Destroy(slot);
        itemSlots.Clear();

        var items = Inventory.Instance.GetAllItems();
        for (int i = 0; i < items.Count; i++)
        {
            var slot = Instantiate(itemSlotPrefab, itemListParent);
            itemSlots.Add(slot);

            Text txt = slot.GetComponentInChildren<Text>();
            if (txt) txt.text = $"{items[i].name} x{items[i].count}";

            int idx = i;
            Button btn = slot.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() =>
                {
                    // 플레이어 Transform을 넘겨줘야 함
                    var player = FindAnyObjectByType<PlayerController>()?.transform;
                    if (player != null) Inventory.Instance.UseItem(idx, player);
                });
        }
    }
}