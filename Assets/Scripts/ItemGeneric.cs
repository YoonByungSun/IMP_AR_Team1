using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName = "GenericItem";
    public Sprite itemIcon;
    public Vector3 rotateSpeed = new Vector3(45f, 90f, 30f);

    protected virtual void Reset()
    {
        // 자식에서 itemName 자동 지정
    }

    protected virtual void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    // 트리거 충돌 시 인벤토리에 추가
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance?.AddItem(itemName, itemIcon);
            Destroy(gameObject);
        }
    }

    // 아이템 사용 인터페이스 (오버라이드용)
    public virtual void Use(Transform player)
    {
        Debug.Log($"{itemName} 사용 (기본)");
    }
}