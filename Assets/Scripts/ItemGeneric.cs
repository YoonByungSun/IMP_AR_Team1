using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    public string itemName = "Generic Item";
    public Vector3 rotateSpeed = new Vector3(45f, 90f, 30f);

    void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Inventory.Instance.AddItem(this);
            gameObject.SetActive(false); // 혹은 Destroy(gameObject);
        }
    }

    public virtual void Use(Transform player)
    {
        Debug.Log($"{itemName} 오버라이드 필요");
    }
}
