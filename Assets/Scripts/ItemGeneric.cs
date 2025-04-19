using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    // 스폰 시 자동 설정됨. 수정 불필요
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
