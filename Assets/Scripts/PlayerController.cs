using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject spawnedPlayer;

    public float scale = 1f;
    public Vector3 defaultScale = new Vector3(0.01f, 0.01f, 0.01f); // ✅ 적과 동일한 기본 스케일로 조정
    public GameObject fkillerEffect;
    private bool isFkillerActive = false;
    private bool isDead = false;
    private int bossKillCount = 0;

    private ARTrackedImageManager trackedImageManager;

    void Awake()
    {
        trackedImageManager = FindAnyObjectByType<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            if (spawnedPlayer == null)
            {
                spawnedPlayer = Instantiate(playerPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
                spawnedPlayer.tag = "Player";
                spawnedPlayer.transform.localScale = defaultScale; // ✅ 처음 생성 시 기본 스케일 적용
            }
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (spawnedPlayer != null)
            {
                spawnedPlayer.transform.position = trackedImage.transform.position;
                spawnedPlayer.transform.rotation = trackedImage.transform.rotation;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy == null) return;

            if (isFkillerActive)
            {
                Destroy(other.gameObject);
                return;
            }

            switch (enemy.enemyType)
            {
                case EnemyController.EnemyType.Mushnub:
                    ScaleUp(0.1f);
                    Destroy(other.gameObject);
                    break;
                case EnemyController.EnemyType.GreenBlob:
                    if (scale >= 1.5f)
                    {
                        ScaleUp(0.2f);
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
                case EnemyController.EnemyType.AlienBlob:
                    if (scale >= 2.5f)
                    {
                        ScaleUp(0.3f);
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        GameOver();
                    }
                    break;
            }
        }

        if (other.CompareTag("Boss"))
        {
            if (scale >= 4.0f)
            {
                Destroy(other.gameObject);
                bossKillCount++;

                if (bossKillCount >= 2)
                {
                    SceneManager.LoadScene("GameClearScene");
                }
            }
            else
            {
                GameOver();
            }
        }
    }

    void ScaleUp(float amount)
    {
        scale = Mathf.Min(scale + amount, 4.0f);
        if (spawnedPlayer != null)
            spawnedPlayer.transform.localScale = defaultScale * scale; // ✅ 기준 스케일에 비례해서 커짐

        if (scale >= 1.5f && SceneManager.GetActiveScene().name == "Stage1")
        {
            SceneManager.LoadScene("Stage2");
        }
        else if (scale >= 2.5f && SceneManager.GetActiveScene().name == "Stage2")
        {
            SceneManager.LoadScene("Stage3");
        }
        else if (scale >= 3.0f && SceneManager.GetActiveScene().name == "Stage3")
        {
            FindObjectOfType<EnemySpawner>()?.SpawnBosses();
        }
    }

    void GameOver()
    {
        if (isDead) return;
        isDead = true;
        SceneManager.LoadScene("GameOverScene");
    }

    public void ActivateFkiller()
    {
        isFkillerActive = true;
        if (fkillerEffect != null)
            fkillerEffect.SetActive(true);
        Invoke("DeactivateFkiller", 5f);
    }

    void DeactivateFkiller()
    {
        isFkillerActive = false;
        if (fkillerEffect != null)
            fkillerEffect.SetActive(false);
    }
}