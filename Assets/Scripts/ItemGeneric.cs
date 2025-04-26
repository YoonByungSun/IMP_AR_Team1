using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    public string itemName = "GenericItem";
    public Vector3 rotateSpeed = new Vector3(45f, 90f, 30f);

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 인벤토리에 직접 추가
            if (Inventory.Instance != null)
            {
                Inventory.Instance.AddItem(itemName);
                Debug.Log($"{itemName} 아이템 인벤토리에 추가됨 (ItemGeneric)");
            }
            Destroy(gameObject);
        }
    }
}
