using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Transform roomTransform;
    private Transform player;

    public int spawnCount = 20;

    void Start()
    {
        StartCoroutine(WaitForPlayerAndSpawn());
    }

    IEnumerator WaitForPlayerAndSpawn()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;

            yield return null;
        }

        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        Renderer[] renderers = roomTransform.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 roomMin = combinedBounds.min;
        Vector3 roomMax = combinedBounds.max;

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomMin.x, roomMax.x),
                combinedBounds.center.y + 0.3f,
                Random.Range(roomMin.z, roomMax.z)
            );

            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyController>().SetInitialDirection(player.position);
        }
    }

    public void SpawnBosses()
    {
        Renderer[] renderers = roomTransform.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds combinedBounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            combinedBounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 roomMin = combinedBounds.min;
        Vector3 roomMax = combinedBounds.max;

        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomMin.x, roomMax.x),
                combinedBounds.center.y + 0.3f,
                Random.Range(roomMin.z, roomMax.z)
            );

            GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            boss.GetComponent<BossController>().player = player;
        }
    }
}
