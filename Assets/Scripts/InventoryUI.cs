using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public Transform itemListParent;
    public GameObject itemSlotPrefab;
    public Sprite emptySlotSprite;  // [추가] 빈 슬롯 기본 이미지

    private List<GameObject> itemSlots = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RefreshUI()
    {
        foreach (var slot in itemSlots)
        {
            Destroy(slot);
        }
        itemSlots.Clear();

        var items = Inventory.Instance.GetAllItems();
        for (int i = 0; i < items.Count; i++)
        {
            int idx = i;

            var slot = Instantiate(itemSlotPrefab, itemListParent);
            itemSlots.Add(slot);

            // 수량 텍스트 설정
            Text txt = slot.GetComponentInChildren<Text>();
            if (txt)
                txt.text = $"X {items[idx].count}";

            // 아이콘 이미지 설정
            Image img = slot.GetComponentInChildren<Image>();
            if (img)
            {
                if (items[idx].icon != null)
                    img.sprite = items[idx].icon;
            }

            // 버튼 클릭 설정
            Button btn = slot.GetComponent<Button>();
            if (btn != null)
                btn.onClick.AddListener(() =>
                {
                    var player = FindAnyObjectByType<PlayerController>()?.transform;
                    if (player != null)
                        Inventory.Instance.UseItem(idx, player);
                });
        }

        // 인벤토리에 아이템이 하나도 없으면 빈 슬롯 표시
        if (items.Count == 0)
        {
            var emptySlot = Instantiate(itemSlotPrefab, itemListParent);
            itemSlots.Add(emptySlot);

            Text txt = emptySlot.GetComponentInChildren<Text>();
            if (txt) txt.text = "";

            Image img = emptySlot.GetComponentInChildren<Image>();
            if (img) img.sprite = emptySlotSprite;
        }
    }

    public void UpdateSingleItemUI(string itemName, int newCount)
    {
        foreach (var slot in itemSlots)
        {
            Text txt = slot.GetComponentInChildren<Text>();
            if (txt != null && txt.text.Contains(itemName))
            {
                if (newCount > 0)
                {
                    txt.text = $"X {newCount}";
                }
                else
                {
                    // 수량이 0이면 전체 새로 그리기
                    RefreshUI();
                }
                return;
            }
        }
    }
}

