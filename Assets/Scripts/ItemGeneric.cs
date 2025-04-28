using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "GenericItem";
    public Sprite itemIcon;
    public Vector3 rotateSpeed = new Vector3(45f, 90f, 30f);

    protected virtual void Reset()
    {
        // �ڽĿ��� itemName �ڵ� ����
    }

    protected virtual void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    // Ʈ���� �浹 �� �κ��丮�� �߰�
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance?.AddItem(itemName, itemIcon);
            Destroy(gameObject);
        }
    }

    // ������ ��� �������̽� (�������̵��)
    public virtual void Use(Transform player)
    {
        Debug.Log($"{itemName} ��� (�⺻)");
    }
}