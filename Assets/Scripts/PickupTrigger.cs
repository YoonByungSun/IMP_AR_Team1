using UnityEngine;

// Function: Trigger to pick up an item and add to inventory
public class PickupTrigger : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(itemName, itemIcon);
            Destroy(gameObject);
        }
    }
}

