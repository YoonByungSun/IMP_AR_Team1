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
            // �κ��丮�� ���� �߰�
            if (Inventory.Instance != null)
            {
                Inventory.Instance.AddItem(itemName);
                Debug.Log($"{itemName} ������ �κ��丮�� �߰��� (ItemGeneric)");
            }
            Destroy(gameObject);
        }
    }
}
