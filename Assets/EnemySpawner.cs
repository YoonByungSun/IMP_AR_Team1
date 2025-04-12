using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnRadius = 2f;

    private Transform playerTransform;

    void Update()
    {
        // �÷��̾� ������Ʈ�� ������ ã�Ƽ� ���
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        // �÷��̾ �����ϰ�, ���� �� ���� �����ϸ� ���� ����
        if (playerTransform != null && GetCurrentEnemyCount() < enemyCount)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 randomPos = playerTransform.position + Random.onUnitSphere * spawnRadius;
        randomPos.y = playerTransform.position.y; // ���� ��ġ ����

        GameObject enemy = Instantiate(enemyPrefab, randomPos, Quaternion.identity);
        enemy.GetComponent<EnemyController>().SetTarget(playerTransform);
    }

    int GetCurrentEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
