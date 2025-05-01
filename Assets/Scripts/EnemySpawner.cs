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
    private GameObject spawned;

    public float spawnMargin = 0.15f;

    void Start()
    {
        StartCoroutine(WaitForPlayerAndSpawn());
        spawned = new GameObject("Enemy");
    }

    IEnumerator WaitForPlayerAndSpawn()
    {
        // 기존 로직 유지
        while (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;

            yield return null;
        }

        while (roomTransform == null)
        {
            GameObject roomObj = GameObject.FindWithTag("Room");
            if (roomObj != null)
                roomTransform = roomObj.transform;

            yield return null;
        }

        // 🎯 현재 로드된 스테이지 씬 중 하나를 활성 씬으로 설정
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Stage1" || scene.name == "Stage2" || scene.name == "Stage3")
            {
                SceneManager.SetActiveScene(scene); // ✅ 이후 Instantiate용
                SceneManager.MoveGameObjectToScene(spawned, scene); // ✅ 부모 오브젝트도 Stage 씬으로 이동
                Debug.Log($"[EnemySpawner] ActiveScene set to {scene.name} / 'Enemy' container moved.");
                break;
            }
        }


        // 이후부터 생성되는 오브젝트는 위에서 활성화한 씬에 귀속됨
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
        if (!roomTransform.TryGetComponent(out BoxCollider roomCollider)) return;

        int count = Random.Range(minSpawn, maxSpawn + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = GetRandomPointInsideRoom(roomCollider);
            spawnPos.y = PlayerSpawner.fixedPlayerY; //  Y값 고정

            int randomIndex = Random.Range(0, enemyPrefabs.Length);
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex], spawnPos, Quaternion.identity);

            if (enemy.TryGetComponent(out EnemyController ec))
            {
                ec.SetInitialDirection(player.position);
            }
        }
    }

    private Vector3 GetRandomPointInsideRoom(BoxCollider box)
    {
        Vector3 size = box.size;
        Vector3 center = box.center;

        float marginX = Mathf.Clamp(spawnMargin, 0f, size.x / 2f);
        float marginZ = Mathf.Clamp(spawnMargin, 0f, size.z / 2f);

        // ✅ 로컬 좌표 기준 랜덤 위치
        float randX = Random.Range(-size.x / 2f + marginX, size.x / 2f - marginX);
        float randZ = Random.Range(-size.z / 2f + marginZ, size.z / 2f - marginZ);

        Vector3 localPoint = new Vector3(randX, 0f, randZ);

        // ✅ BoxCollider의 회전/스케일 모두 반영된 월드 좌표
        Vector3 worldPoint = box.transform.TransformPoint(center + localPoint);
        worldPoint.y = PlayerSpawner.fixedPlayerY;

        return worldPoint;
    }






    public void SpawnBosses()
    {
        if (player == null || roomTransform == null) return;
        if (!roomTransform.TryGetComponent(out Collider roomCollider)) return;

        Bounds bounds = roomCollider.bounds;
        Vector3 roomMin = bounds.min;
        Vector3 roomMax = bounds.max;
        float yPos = PlayerSpawner.fixedPlayerY;

        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPos = new Vector3(
                Random.Range(roomMin.x + spawnMargin, roomMax.x - spawnMargin),
                yPos,
                Random.Range(roomMin.z + spawnMargin, roomMax.z - spawnMargin)
            );

            GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

            if (boss.TryGetComponent(out BossController bc))
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

