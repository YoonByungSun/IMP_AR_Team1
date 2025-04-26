using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Transform roomTransform;
    private Transform player;

    public int spawnCount = 20;
    public float spawnMargin = 0.3f;
    private bool hasSpawnedBoss = false;

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
        string currentScene = SceneManager.GetActiveScene().name;
       if(currentScene=="Stage1" || currentScene=="Stage2")
        {
            SpawnEnemies();
        }
       else if(currentScene=="Stage3")
        {
            StartCoroutine(SpawnEnemiesUnitScaleLimit());
            SpawnBosses();
        }
      
    }
    IEnumerator SpawnEnemiesUnitScaleLimit()
    {
        while(true)
        {
            if(PlayerData.Instance!=null&&PlayerData.Instance.savedScale>=0.5f)
            {
                Debug.Log("scale 0.5이상, 일반 적 스폰 종료");
                yield break;
            }
            SpawnEnemies();
            yield return new WaitForSeconds(5f);
        }
    }

    public void SpawnEnemies()
    {
        if (player == null) return;
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;

        Bounds bounds = roomCollider.bounds;
        Vector3 roomMin = bounds.min;
        Vector3 roomMax = bounds.max;

        float yPos = 0.1f; // ✅ 플레이어와 동일하게 Y 고정

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomMin.x + spawnMargin, roomMax.x - spawnMargin), // X 랜덤
                yPos,                                                            // Y 고정
                Random.Range(roomMin.z + spawnMargin, roomMax.z - spawnMargin)  // Z 랜덤
            );

            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

            if (enemy.TryGetComponent(out EnemyController ec) && player != null)
            {
                ec.SetInitialDirection(player.position);
            }
        }
    }


    public void SpawnBosses()
    {
        if (player == null) return;
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;

        Bounds bounds = roomCollider.bounds;
        Vector3 roomMin = bounds.min;
        Vector3 roomMax = bounds.max;
        float yPos = roomMin.y + 0.3f;

        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomMin.x + spawnMargin, roomMax.x - spawnMargin),
                yPos,
                Random.Range(roomMin.z + spawnMargin, roomMax.z - spawnMargin)
            );

            GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            if (boss.TryGetComponent(out BossController bc) && player != null)
            {
                bc.player = player;
            }
        }
    }
}
