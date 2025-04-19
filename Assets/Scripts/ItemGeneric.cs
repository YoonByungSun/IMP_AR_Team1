using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    // ���� �� �ڵ� ������. ���� ���ʿ�
    public string itemName = "GenericItem";
    public float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.AddItem(itemName, this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
