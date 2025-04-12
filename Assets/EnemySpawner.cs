using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public Transform player;

    void Start()
    {
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
            enemy.GetComponent<EnemyController>().SetInitialDirection(player.position);
        }
    }
}
