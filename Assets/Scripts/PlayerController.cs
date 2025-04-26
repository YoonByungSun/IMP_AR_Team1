using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float scale = 1f;
    public Vector3 defaultScale = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject fkillerEffect;

    private bool isFkillerActive = false;
    private bool isDead = false;
    private int bossKillCount = 0;
    private GameObject spawnedPlayer;

    // 생명 관련 변수
    public int maxLife = 3;
    private int currentLife;
    public GameObject heartPrefab;
    public Transform heartParent;
    private List<GameObject> hearts = new List<GameObject>();

    // UI 관련 변수
    public GameObject retryButton;
    public Text gameOverText;

    private void Start()
    {
        // Stage1이 아닐 때 저장된 스케일 적용
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Stage1" && PlayerData.Instance != null)
        {
            scale = PlayerData.Instance.savedScale;
            transform.localScale = new Vector3(scale, scale, scale);
            Debug.Log("PlayerController: savedScale applied = " + scale);
        }

        // 생명 초기화 및 하트 UI 생성
        currentLife = maxLife;
        retryButton.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        CreateHearts();
    }

    void CreateHearts()
    {
        // 하트 오브젝트를 생성하고 화면에 배치
        for (int i = 0; i < maxLife; i++)
        {
            GameObject heart = Instantiate(heartPrefab, heartParent);
            heart.GetComponent<RectTransform>().anchoredPosition = new Vector2(60 * i, 0);
            hearts.Add(heart);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision detected: " + other.name + ", tag = " + other.tag);

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy == null)
            {
                Debug.LogWarning("Enemy tag detected but no EnemyController attached.");
                return;
            }

            Debug.Log("Enemy type: " + enemy.enemyType);

            switch (enemy.enemyType)
            {
                case EnemyController.EnemyType.Mushnub:
                    ScaleUp(0.01f);
                    Destroy(other.gameObject);
                    break;

                case EnemyController.EnemyType.GreenBlob:
                    if (scale >= 0.06f)
                    {
                        ScaleUp(0.02f);
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        LoseLife();
                    }
                    break;

                case EnemyController.EnemyType.AlienBlob:
                    if (scale >= 0.2f)
                    {
                        ScaleUp(0.03f);
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        LoseLife();
                    }
                    break;

                default:
                    Debug.LogError("Unknown enemyType.");
                    break;
            }
        }

        if (other.CompareTag("Boss"))
        {
            if (scale >= 0.35f)
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
                LoseLife();
            }
        }
    }

    void ScaleUp(float amount)
    {
        // 플레이어 스케일 증가
        scale = Mathf.Min(scale + amount, 1.0f);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.savedScale = scale;
            Debug.Log("Scale saved: " + scale);
        }

        // 스테이지 전환 처리
        string currentScene = SceneManager.GetActiveScene().name;
        if (scale >= 0.06f && currentScene == "Stage1")
            SceneManager.LoadScene("Stage2");
        else if (scale >= 0.2f && currentScene == "Stage2")
            SceneManager.LoadScene("Stage3");
    }

    void LoseLife()
    {
        // 생명 감소 처리
        if (isDead || currentLife <= 0) return;

        currentLife--;

        if (currentLife >= 0 && currentLife < hearts.Count)
        {
            Destroy(hearts[currentLife]);
            hearts.RemoveAt(currentLife);
        }

        if (currentLife <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        // 게임 오버 처리
        if (isDead) return;

        isDead = true;

        if (gameOverText != null)
            gameOverText.gameObject.SetActive(true);

        if (retryButton != null)
            retryButton.SetActive(true);
    }

    public void RetryGame()
    {
        // 현재 씬 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ActivateFkiller()
    {
        // Fkiller 기능 활성화
        isFkillerActive = true;
        if (fkillerEffect != null)
            fkillerEffect.SetActive(true);
        Invoke("DeactivateFkiller", 5f);
    }

    void DeactivateFkiller()
    {
        // Fkiller 기능 비활성화
        isFkillerActive = false;
        if (fkillerEffect != null)
            fkillerEffect.SetActive(false);
    }
}



