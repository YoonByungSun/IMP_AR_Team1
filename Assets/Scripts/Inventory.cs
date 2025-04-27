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
        Debug.Log($"{item.itemName} �κ��丮�� �߰���!");
        InventoryUI.Instance?.RefreshUI();
    }

    public void UseItem(int index)
    {
        if (index < 0 || index >= items.Count) return;
        items[index].Use();
        items.RemoveAt(index);
        InventoryUI.Instance?.RefreshUI();
    }

    public List<ItemGeneric> GetAllItems()
    {
        return new List<ItemGeneric>(items);
    }
}
