using System.Collections;
using UnityEngine;

// Function: Randomly Spawn Items over the Room
public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform roomTransform;
    public float spawnMargin = 0.3f;
    public int maxCount = 5;
    public float minDelay = 1f;
    public float maxDelay = 5f;
    public float spawnRate = 0.5f; // 0~1 사이의 확률

    void Start()
    {
        StartCoroutine(startSpawn());
    }

    IEnumerator startSpawn()
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
        float yPos = 0.55f;

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
                Instantiate(itemPrefabs[randomIndex], spawnPos, Quaternion.identity);
                spawned++;
            }
        }
    }
}