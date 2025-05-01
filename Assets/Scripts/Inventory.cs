using System.Collections.Generic;
using UnityEngine;

// Function: Manage Collected Items
public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<Item> items = new List<Item>();

    public struct Item
    {
        public string name;
        public int count;
        public Sprite icon;  // [�߰�] ������ ����

        public Item(string name, int count = 1, Sprite icon = null)
        {
            this.name = name;
            this.count = count;
            this.icon = icon;
        }
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ������ �߰�
    public void AddItem(string itemName, Sprite itemIcon = null)
    {
        int idx = items.FindIndex(x => x.name == itemName);
        if (idx != -1)
        {
            items[idx] = new Item(items[idx].name, items[idx].count + 1, items[idx].icon);
        }
        else
        {
            items.Add(new Item(itemName, 1, itemIcon));
        }

        InventoryUI.Instance?.UpdateSingleItemUI(itemName, items[idx != -1 ? idx : items.Count - 1].count);


    }

    // For UI only, Use() function in each Item Script, ItemGeneric.cs
    public void UseItem(int index, Transform player)
    {
        if (index < 0 || index >= items.Count) return;
        string itemName = items[index].name;

        ItemGeneric itemScript = player.GetComponent<ItemGeneric>();
        if (itemScript && itemScript.itemName == itemName)
        {
            itemScript.Use(player);
            
        }
        else
        {
            switch (itemName)
            {
                case "Spray":
                    // �κ��丮 �� ������ �����ܿ��� ItemSpray.cs �־����
                    var spray = player.gameObject.AddComponent<ItemSpray>();
                    spray.Use(player);
                    Destroy(spray);
                    break;
                default:
                    Debug.LogError($"{itemName} no Script");
                    break;
            }

            var anim = player.GetComponent<PlayerAnimatorController>();
            anim?.PlayCheer();
        }

        // ���� ����
        Item updatedItem = items[index];
        updatedItem.count--;
        if (updatedItem.count <= 0)
            items.RemoveAt(index);
        else
            items[index] = updatedItem;

        InventoryUI.Instance?.RefreshUI();
    }

    public List<Item> GetAllItems()
    {
        return new List<Item>(items);
    }
}
