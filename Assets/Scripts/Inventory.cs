using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public List<string> items = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환에도 파괴되지 않음
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string itemName)
    {
        items.Add(itemName);
        Debug.Log("인벤토리 추가: " + itemName);
    }

    public void RemoveItem(string itemName)
    {
        items.Remove(itemName);
        Debug.Log("인벤토리 제거: " + itemName);
    }

    public bool HasItem(string itemName)
    {
        return items.Contains(itemName);
    }
}
