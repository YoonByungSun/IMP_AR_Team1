using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnRadius = 2f;

    private Transform playerTransform;

    void Update()
    {
        // 플레이어 오브젝트가 없으면 찾아서 등록
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        // 플레이어가 존재하고, 현재 적 수가 부족하면 새로 생성
        if (playerTransform != null && GetCurrentEnemyCount() < enemyCount)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = playerTransform.position + Random.onUnitSphere * spawnRadius;
        randomPos.y = playerTransform.position.y; // 평면상 위치 고정

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemy.GetComponent<EnemyController>().SetTarget(playerTransform);
    }

    int GetCurrentEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
