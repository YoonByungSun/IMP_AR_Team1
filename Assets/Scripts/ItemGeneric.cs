using UnityEngine;

// Function: General Items' Settings
public class ItemGeneric : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "GenericItem";
    public Sprite itemIcon;
    public Vector3 rotateSpeed = new Vector3(45f, 90f, 30f);

    protected virtual void Reset()
    {
        // Set from each Item Script (Override)
    }
    
    protected virtual void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance?.AddItem(itemName, itemIcon);
            Destroy(gameObject);
        }
    }

    // Item Interface (Override)
    public virtual void Use(Transform player)
    {
        Debug.LogError($"{itemName} Use() not set");
    }
}