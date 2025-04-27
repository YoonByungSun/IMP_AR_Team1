using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemPrefabs; // 여러 아이템 프리팹
    public Transform roomTransform;
    public int spawnCount = 5;
    public float spawnMargin = 0.3f;
    private bool hasSpawned = false;

    void Start()
    {
        StartCoroutine(WaitForRoomAndSpawn());
    }

    IEnumerator WaitForRoomAndSpawn()
    {
        // roomTransform 설정이 늦게 될 수 있음
        while (roomTransform == null)
        {
            yield return null;
        }

        SpawnItems();
    }

    public void SpawnItems()
    {
        if (roomTransform == null) return;
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;

        Bounds bounds = roomCollider.bounds;
        Vector3 roomMin = bounds.min;
        Vector3 roomMax = bounds.max;

        float yPos = 0.1f; // ✅ 플레이어/적과 동일하게 Y 고정

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
