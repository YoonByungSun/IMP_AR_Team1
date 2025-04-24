using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float scale = 1f;
    public Vector3 defaultScale = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject fkillerEffect;

    private bool isFkillerActive = false;
    private bool isDead = false;
    private int bossKillCount = 0;
    private GameObject spawnedPlayer;

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[충돌 발생] other.name = {other.name}, tag = {other.tag}");

        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy == null)
            {
                Debug.LogWarning("Enemy 태그인데 EnemyController가 없음");
                return;
            }

            Debug.Log($"[EnemyController 확인] enemyType = {enemy.enemyType}");

            switch (enemy.enemyType)
            {
                case EnemyController.EnemyType.Mushnub:
                    Debug.Log("✅ Mushnub과 충돌 → ScaleUp");
                    ScaleUp(0.02f);
                    Destroy(other.gameObject);
                    break;

                case EnemyController.EnemyType.GreenBlob:
                    Debug.Log("🟢 GreenBlob과 충돌");
                    if (scale >= 0.4f)
                    {
                        ScaleUp(0.04f);
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        Debug.Log("🛑 GreenBlob 조건 미달 → GameOver");
                        GameOver();
                    }
                    break;

                case EnemyController.EnemyType.AlienBlob:
                    Debug.Log("👽 AlienBlob과 충돌");
                    if (scale >= 0.6f)
                    {
                        ScaleUp(0.08f);
                        Destroy(other.gameObject);
                    }
                    else
                    {
                        GameOver();
                    }
                    break;

                default:
                    Debug.LogError("❗알 수 없는 enemyType");
                    break;
            }
        }
    


        if (other.CompareTag("Boss"))
        {
            if (scale >= 1f)
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

        // ✅ 현재 오브젝트의 스케일 증가
        transform.localScale += new Vector3(amount, amount, amount);

        // ✅ Y 위치 고정
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, 0.1f, pos.z);

        // ✅ 스테이지 조건 유지
        if (scale >= 0.4f && SceneManager.GetActiveScene().name == "Stage1")
            SceneManager.LoadScene("Stage2");
        else if (scale >= 0.6f && SceneManager.GetActiveScene().name == "Stage2")
            SceneManager.LoadScene("Stage3");
        else if (scale >= 1f && SceneManager.GetActiveScene().name == "Stage3")
            FindObjectOfType<EnemySpawner>()?.SpawnBosses();
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

