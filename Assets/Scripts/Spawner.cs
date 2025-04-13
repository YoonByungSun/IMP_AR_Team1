using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;

    private Transform player;

    public void StartSpawning(Transform playerTransform)
    {
        player = playerTransform;
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float x = Random.Range(GameManager.Instance.planeMin.x, GameManager.Instance.planeMax.x);
            float z = Random.Range(GameManager.Instance.planeMin.y, GameManager.Instance.planeMax.y);
            Vector3 spawnPos = new Vector3(x, 0f, z);

            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            enemy.GetComponent<EnemyBehavior>().SetInitialDirection(player.position);
        }
    }
}
