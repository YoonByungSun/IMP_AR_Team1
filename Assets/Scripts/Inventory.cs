using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<ItemGeneric> items = new List<ItemGeneric>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(ItemGeneric item)
    {
        items.Add(item);
        Debug.Log($"{item.itemName} 인벤토리에 추가됨!");
        InventoryUI.Instance?.RefreshUI();
    }

    public void UseItem(int index)
    {
        Transform player = FindAnyObjectByType<PlayerController>().transform;
        if (index < 0 || index >= items.Count) return;
        items[index].Use(player);
        items.RemoveAt(index);
        InventoryUI.Instance?.RefreshUI();
    }

    public List<ItemGeneric> GetAllItems()
    {
        return new List<ItemGeneric>(items);
    }
}
