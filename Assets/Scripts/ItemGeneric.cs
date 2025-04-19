using UnityEngine;

public class ItemGeneric : MonoBehaviour
{
    // 스폰 시 자동 설정됨. 수정 불필요
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
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.AddItem(itemName, this.gameObject);
                Destroy(gameObject);
            }
        }
    }
}
