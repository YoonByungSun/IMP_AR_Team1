using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Transform roomTransform;
    private Transform player;

    public float spawnMargin = 0.3f;

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

        // room 생성 전에 적 생성 안되도록 수정 2025-04-30
        while (roomTransform == null)
        {
            GameObject roomObj = GameObject.FindWithTag("Room");
            if (roomObj != null)
                roomTransform = roomObj.transform;

            yield return null;
        }


        if (IsSceneLoaded("Stage1") || IsSceneLoaded("Stage2"))
        {
            StartCoroutine(SpawnEnemiesUntilScaleLimit());
        }
        else if (IsSceneLoaded("Stage3"))
        {
            StartCoroutine(SpawnEnemiesUntilScaleLimit());
            SpawnBosses();
        }

    }

    IEnumerator SpawnEnemiesUntilScaleLimit()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        while (true)
        {
            float scale = PlayerData.Instance != null ? PlayerData.Instance.savedScale : 0f;

            if ((IsSceneLoaded("Stage1") && scale >= 0.06f) ||
                (IsSceneLoaded("Stage2") && scale >= 0.2f) ||
                (IsSceneLoaded("Stage3") && scale >= 0.5f))
            {
                Debug.Log("[EnemySpawner] Scale condition met. Stopping enemy spawn.");
                yield break;
            }


            SpawnEnemies(6, 7);
            yield return new WaitForSeconds(3f);
        }
    }

    public void SpawnEnemies(int minSpawn = 1, int maxSpawn = 3)
    {
        if (player == null || roomTransform == null) return;

        Vector3 roomCenter = roomTransform.position;
        Vector3 roomSize = roomTransform.localScale;

        float yPos = PlayerSpawner.fixedPlayerY;
        int count = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomCenter.x - roomSize.x / 2f + spawnMargin, roomCenter.x + roomSize.x / 2f - spawnMargin),
                yPos,
                Random.Range(roomCenter.z - roomSize.z / 2f + spawnMargin, roomCenter.z + roomSize.z / 2f - spawnMargin)
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
        if (player == null || roomTransform == null) return;

        Vector3 roomCenter = roomTransform.position;
        Vector3 roomSize = roomTransform.localScale;

        float yPos = PlayerSpawner.fixedPlayerY;

        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomCenter.x - roomSize.x / 2f + spawnMargin, roomCenter.x + roomSize.x / 2f - spawnMargin),
                yPos,
                Random.Range(roomCenter.z - roomSize.z / 2f + spawnMargin, roomCenter.z + roomSize.z / 2f - spawnMargin)
            );

            GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            if (boss.TryGetComponent(out BossController bc) && player != null)
            {
                bc.player = player;
            }
        }
    }


    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
                return true;
        }
        return false;
    }

}

