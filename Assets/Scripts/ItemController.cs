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

    public List<ItemData> itemList;           // ������ �� �ִ� ������ ���
    private float spawnIntervalMin = 1.5f;       // �ּ� ���� �ð�
    private float spawnIntervalMax = 5f;       // �ִ� ���� �ð�
    private float spawnRadius = 5.0f;          // �÷��̾� ���� ���� �ݰ�

    //private Transform player;
    public GameObject player;

    void Start()
    {
        Debug.Log(player.transform.position);
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
        if (itemList.Count == 0 || player == null) return;

        // ���� ������ ����
        ItemData selectedItem = itemList[Random.Range(0, itemList.Count)];

        // �÷��̾� �ֺ� ���� ��ġ ���
        Vector3 randomOffset = Random.onUnitSphere * spawnRadius;
        randomOffset.y = 0f;
        Vector3 spawnPos = player.transform.position + randomOffset;

        // ������ ����
        Instantiate(selectedItem.prefab, spawnPos, Quaternion.identity, this.transform);
    }
}
