
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public Transform roomTransform;
    private Transform player;
    private GameObject spawned;

    public float spawnMargin = 0.3f;

    void Start()
    {
        StartCoroutine(SpawnReady());
        if (!GameObject.Find("Enemy"))
            spawned = new GameObject("Enemy");
    }

    IEnumerator SpawnReady()
    {
        while (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;

            yield return null;
        }

        while (roomTransform == null)
        {
            roomTransform = RoomSpawner.Instance.GetRoom().transform;
            yield return null;
        }

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Stage1" || scene.name == "Stage2" || scene.name == "Stage3")
            {
                SceneManager.SetActiveScene(scene);
                SceneManager.MoveGameObjectToScene(spawned, scene);
                break;
            }
        }

        if (IsSceneLoaded("Stage1") || IsSceneLoaded("Stage2"))
        {
            StartCoroutine(SpawnRoutine());
        }
        else if (IsSceneLoaded("Stage3"))
        {
            StartCoroutine(SpawnRoutine());
            SpawnBoss();
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float scale = PlayerController.scale;

            if ((IsSceneLoaded("Stage1") && scale >= 0.06f) ||
                (IsSceneLoaded("Stage2") && scale >= 0.2f) ||
                (IsSceneLoaded("Stage3") && scale >= 0.5f))
            {
                yield break;
            }

            SpawnEnemy(6, 7);
            yield return new WaitForSeconds(3f);
        }
    }

    public void SpawnEnemy(int minSpawn = 1, int maxSpawn = 3)
    {
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;
        if (!spawned) return;

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
            enemy.transform.parent = spawned.transform;

            if (enemy.TryGetComponent(out EnemyController ec) && player != null)
            {
                ec.InitDir(player.position);
            }
        }
    }

    public void SpawnBoss()
    {
        if (player == null || roomTransform == null) return;
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;

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
            boss.transform.parent = spawned.transform;

            if (boss.TryGetComponent(out BossController bc) && player != null)
                bc.player = player;
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