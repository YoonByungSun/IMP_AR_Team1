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

    // 아이템 추가
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

    // 아이템 사용 (UseItem에서 인벤토리에서 이름만 꺼냄 → Use 함수 실행)
    public void UseItem(int index, Transform player)
    {
        if (index < 0 || index >= items.Count) return;
        string itemName = items[index].name;

        // 플레이어에게 Attach된 해당 아이템 스크립트 찾아서 Use 실행
        ItemGeneric itemScript = player.GetComponent<ItemGeneric>();
        if (itemScript && itemScript.itemName == itemName)
        {
            itemScript.Use(player);
        }
        else
        {
            // 플레이어 오브젝트에 해당 아이템 스크립트가 없으면 임시 인스턴스 생성 후 사용
            // (이 방식은 상황에 따라 다를 수 있으니, 필요시 커스터마이징)
            switch (itemName)
            {
                case "Spray":
                    var spray = player.gameObject.AddComponent<ItemSpray>();
                    spray.Use(player);
                    Destroy(spray);
                    break;
                // case "다른아이템명": ... break;
                default:
                    Debug.Log($"{itemName} 사용 스크립트 없음");
                    break;
            }
        }

        // 개수 감소
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