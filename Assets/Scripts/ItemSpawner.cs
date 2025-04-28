using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs;
    public Transform roomTransform;
    public int spawnCount = 5;
    public float spawnMargin = 0.3f;

    void Start()
    {
        StartCoroutine(WaitForRoomAndSpawn());
    }

    IEnumerator WaitForRoomAndSpawn()
    {
        while (roomTransform == null) yield return null;
        SpawnItems();
    }

    public void SpawnItems()
    {
        if (roomTransform == null) return;
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;

        Bounds bounds = roomCollider.bounds;
        Vector3 roomMin = bounds.min;
        Vector3 roomMax = bounds.max;
        float yPos = 0.1f;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomMin.x + spawnMargin, roomMax.x - spawnMargin),
                yPos,
                Random.Range(roomMin.z + spawnMargin, roomMax.z - spawnMargin)
            );

            int randomIndex = Random.Range(0, itemPrefabs.Length);
            Instantiate(itemPrefabs[randomIndex], spawnPos, Quaternion.identity);
        }
    }
}