using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
        public GameObject prefab;
    }

    [Header("���� ����")]
    public List<ItemData> itemList;           // ���� ������ ������ �����յ�
    public GameObject player;                 // ���� ���� ��ġ (�÷��̾�)
    public float spawnIntervalMin = 1.5f;
    public float spawnIntervalMax = 5f;
    public float spawnRadius = 5.0f;
    public float minDistance = 1.5f;

    [Header("���� ������ ���� (�����տ� ���� ��츸)")]
    public string itemName = "GenericItem";   // �κ��丮�� ����� �̸�

    private void Start()
    {
        if (itemList.Count > 0 && player != null)
        {
            StartCoroutine(SpawnItemRoutine());
        }
    }

    IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(waitTime);

            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        ItemData selectedItem = itemList[Random.Range(0, itemList.Count)];

        const int maxAttempts = 10;
        Vector3 spawnPos = Vector3.zero;
        bool foundValid = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 offset = Random.onUnitSphere * spawnRadius;
            offset.y = 0f;
            spawnPos = player.transform.position + offset;

            if (IsValidPosition(spawnPos))
            {
                foundValid = true;
                break;
            }
        }

        if (foundValid)
        {
            GameObject newItem = Instantiate(selectedItem.prefab, spawnPos, Quaternion.identity);
            newItem.transform.parent = transform;
        }
    }

    bool IsValidPosition(Vector3 pos)
    {
        foreach (ItemController item in FindObjectsOfType<ItemController>())
        {
            if (Vector3.Distance(item.transform.position, pos) < minDistance)
                return false;
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.AddItem(itemName, this.gameObject);
                Debug.Log(itemName + " ������ �κ��丮�� �߰���");
                Destroy(gameObject); // ���� ������Ʈ�� ����
            }
        }
    }
}
