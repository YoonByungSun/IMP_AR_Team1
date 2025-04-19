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

    [Header("스폰 설정")]
    public List<ItemData> itemList;
    public GameObject player;
    public float spawnIntervalMin = 1.5f;
    public float spawnIntervalMax = 5f;
    public float spawnRadius = 5f;
    public float minDistance = 1.5f;

    private void Start()
    {
        if (itemList.Count > 0 && player != null)
            StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float wait = Random.Range(spawnIntervalMin, spawnIntervalMax);
            yield return new WaitForSeconds(wait);
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        ItemData item = itemList[Random.Range(0, itemList.Count)];

        const int maxAttempts = 10;
        Vector3 spawnPos = Vector3.zero;
        bool valid = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 offset = Random.onUnitSphere * spawnRadius;
            offset.y = 0;
            spawnPos = player.transform.position + offset;

            if (IsValidPosition(spawnPos))
            {
                valid = true;
                break;
            }
        }

        if (valid)
        {
            GameObject obj = Instantiate(item.prefab, spawnPos, Quaternion.Euler(0, 45, 0));
            obj.tag = "Item";
            obj.transform.parent = transform;
            ItemGeneric i = obj.GetComponent<ItemGeneric>();
            if (i != null)
                i.itemName = item.itemName;
            Debug.Log("New Item (" + item.itemName + ") spawned at " + spawnPos);
        }
    }

    bool IsValidPosition(Vector3 pos)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
        {
            if (Vector3.Distance(obj.transform.position, pos) < minDistance)
                return false;
        }
        return true;
    }
}
