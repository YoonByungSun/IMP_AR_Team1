using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public Transform canvasParent;        // ex: Canvas 오브젝트
    public GameObject itemBoxPrefab;      // ex: ItemBox 프리팹
    public Sprite emptySlotSprite;        // 빈 슬롯일 때 기본 아이콘

    private List<GameObject> itemSlots = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RefreshUI()
    {
        // 기존 슬롯 삭제
        foreach (var slot in itemSlots)
        {
            Destroy(slot);
        }
        itemSlots.Clear();

        var items = Inventory.Instance.GetAllItems();

        for (int i = 0; i < items.Count; i++)
        {
            int idx = i;
            var slot = Instantiate(itemBoxPrefab, canvasParent);
            itemSlots.Add(slot);

            // 아이콘 설정
            var icon = slot.transform.Find("ItemIcon")?.GetComponent<Image>();
            if (icon != null && items[idx].icon != null)
                icon.sprite = items[idx].icon;

            // 수량 텍스트 설정
            var countText = slot.transform.Find("ItemCountText")?.GetComponent<Text>();
            if (countText != null)
                countText.text = $"X {items[idx].count}";

            // 버튼 설정
            var button = slot.transform.Find("ItemButton")?.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(() =>
                {
                    var player = FindAnyObjectByType<PlayerController>()?.transform;
                    if (player != null)
                        Inventory.Instance.UseItem(idx, player);
                });
        }

        // 빈 슬롯 처리
        if (items.Count == 0)
        {
            var emptySlot = Instantiate(itemBoxPrefab, canvasParent);
            itemSlots.Add(emptySlot);

            var icon = emptySlot.transform.Find("ItemIcon")?.GetComponent<Image>();
            if (icon != null)
                icon.sprite = emptySlotSprite;

            var countText = emptySlot.transform.Find("ItemCountText")?.GetComponent<Text>();
            if (countText != null)
                countText.text = "";

            // 버튼 비활성화 (클릭 방지)
            var button = emptySlot.transform.Find("ItemButton")?.GetComponent<Button>();
            if (button != null)
                button.interactable = false;
        }
    }
}




