using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform roomTransform;
    public float spawnMargin = 0.3f;
    public int maxCount = 5;
    public float minDelay = 1f;
    public float maxDelay = 5f;
    public float spawnRate = 0.5f;

    private GameObject spawnedItem;

    void Start()
    {
        if (GameObject.Find("Item") == null)
            spawnedItem = new GameObject("Item");
        StartCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        while (roomTransform == null)
        {
            GameObject roomObj = GameObject.FindWithTag("Room");
            if (roomObj != null)
                roomTransform = roomObj.transform;

            yield return null;
        }

        StartCoroutine(SpawnItems());
    }

    IEnumerator SpawnItems()
    {
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) yield break;

        Bounds bounds = roomCollider.bounds;
        Vector3 roomMin = bounds.min;
        Vector3 roomMax = bounds.max;
        float yPos = PlayerSpawner.fixedPlayerY;

        int spawned = 0;

        while (spawned < maxCount)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            if (Random.value <= spawnRate)
            {
                Vector3 spawnPos = new Vector3(
                    Random.Range(roomMin.x + spawnMargin, roomMax.x - spawnMargin),
                    yPos,
                    Random.Range(roomMin.z + spawnMargin, roomMax.z - spawnMargin)
                );

                int randomIndex = Random.Range(0, itemPrefabs.Length);
                GameObject item = Instantiate(itemPrefabs[randomIndex], spawnPos, Quaternion.identity);
                item.transform.parent = spawnedItem.transform;

                spawned++;
            }
        }
    }
}