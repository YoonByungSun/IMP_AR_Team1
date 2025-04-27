using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    public GameObject itemButtonPrefab;
    public Transform contentParent;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        List<ItemGeneric> items = Inventory.Instance.GetAllItems();

        for (int i = 0; i < items.Count; i++)
        {
            int idx = i;
            GameObject btnObj = Instantiate(itemButtonPrefab, contentParent);
            btnObj.GetComponentInChildren<Text>().text = items[i].itemName;
            btnObj.GetComponent<Button>().onClick.AddListener(() => OnItemClicked(idx));
        }
    }

    void OnItemClicked(int index)
    {
        Inventory.Instance.UseItem(index);
        RefreshUI();
    }
}
