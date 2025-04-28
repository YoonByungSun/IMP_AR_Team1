using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    private List<Item> items = new List<Item>();

    public struct Item
    {
        public string name;
        public int count;
        public Item(string name, int count = 1)
        {
            this.name = name;
            this.count = count;
        }
    }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ������ �߰�
    public void AddItem(string itemName)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == itemName)
            {
                Item updated = items[i];
                updated.count++;
                items[i] = updated;
                InventoryUI.Instance?.RefreshUI();
                return;
            }
        }
        items.Add(new Item(itemName, 1));
        InventoryUI.Instance?.RefreshUI();
    }

    // ������ ��� (UseItem���� �κ��丮���� �̸��� ���� �� Use �Լ� ����)
    public void UseItem(int index, Transform player)
    {
        if (index < 0 || index >= items.Count) return;
        string itemName = items[index].name;

        // �÷��̾�� Attach�� �ش� ������ ��ũ��Ʈ ã�Ƽ� Use ����
        ItemGeneric itemScript = player.GetComponent<ItemGeneric>();
        if (itemScript && itemScript.itemName == itemName)
        {
            itemScript.Use(player);
        }
        else
        {
            // �÷��̾� ������Ʈ�� �ش� ������ ��ũ��Ʈ�� ������ �ӽ� �ν��Ͻ� ���� �� ���
            // (�� ����� ��Ȳ�� ���� �ٸ� �� ������, �ʿ�� Ŀ���͸���¡)
            switch (itemName)
            {
                case "Spray":
                    var spray = player.gameObject.AddComponent<ItemSpray>();
                    spray.Use(player);
                    Destroy(spray);
                    break;
                // case "�ٸ������۸�": ... break;
                default:
                    Debug.Log($"{itemName} ��� ��ũ��Ʈ ����");
                    break;
            }
        }

        // ���� ����
        Item updatedItem = items[index];
        updatedItem.count--;
        if (updatedItem.count <= 0) items.RemoveAt(index);
        else items[index] = updatedItem;

        InventoryUI.Instance?.RefreshUI();
    }

    public List<Item> GetAllItems()
    {
        return new List<Item>(items);
    }
}