using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public Transform canvasParent;        // ex: Canvas ������Ʈ
    public GameObject itemBoxPrefab;      // ex: ItemBox ������
    public Sprite emptySlotSprite;        // �� ������ �� �⺻ ������

    private List<GameObject> itemSlots = new List<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RefreshUI()
    {
        // ���� ���� ����
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

            // ������ ����
            var icon = slot.transform.Find("ItemIcon")?.GetComponent<Image>();
            if (icon != null && items[idx].icon != null)
                icon.sprite = items[idx].icon;

            // ���� �ؽ�Ʈ ����
            var countText = slot.transform.Find("ItemCountText")?.GetComponent<Text>();
            if (countText != null)
                countText.text = $"X {items[idx].count}";

            // ��ư ����
            var button = slot.transform.Find("ItemButton")?.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(() =>
                {
                    var player = FindAnyObjectByType<PlayerController>()?.transform;
                    if (player != null)
                        Inventory.Instance.UseItem(idx, player);
                });
        }

        // �� ���� ó��
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

            // ��ư ��Ȱ��ȭ (Ŭ�� ����)
            var button = emptySlot.transform.Find("ItemButton")?.GetComponent<Button>();
            if (button != null)
                button.interactable = false;
        }
    }
}




