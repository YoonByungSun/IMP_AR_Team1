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
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Stage1이 아닐 때만 저장된 스케일 적용
        if (currentScene != "Stage1" && PlayerData.Instance != null)
        {
            scale = PlayerData.Instance.savedScale;
            transform.localScale = new Vector3(scale, scale, scale);
            Debug.Log($"📌 PlayerController: savedScale 적용됨 = {scale}");
        }
    }



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
                    ScaleUp(0.01f);
                    Destroy(other.gameObject);
                    break;

                case EnemyController.EnemyType.GreenBlob:
                    Debug.Log("🟢 GreenBlob과 충돌");
                    if (scale >= 0.06f)
                    {
                        ScaleUp(0.02f);
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
                    if (scale >= 0.2f)
                    {
                        ScaleUp(0.03f);
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
                GameOver();
            }
        }
    }

    void ScaleUp(float amount)
    {
        scale = Mathf.Min(scale + amount, 1.0f); // 최대 크기 제한은 네가 정하기 나름

        transform.localScale = new Vector3(scale, scale, scale);  // 실제 크기로 적용
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        // 저장
        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.savedScale = scale;
            Debug.Log($"✅ 스케일 저장됨: {scale}");
        }

        string currentScene = SceneManager.GetActiveScene().name;

        if (scale >= 0.06f && currentScene == "Stage1")
            SceneManager.LoadScene("Stage2");
        else if (scale >= 0.2f && currentScene == "Stage2")
            SceneManager.LoadScene("Stage3");
        
    }




    void GameOver()
    {
        if (isDead) return;
        isDead = true;
        SceneManager.LoadScene("GameOverScene");
    }

    //public void ActivateFkiller()
    //{
    //    isFkillerActive = true;
    //    if (fkillerEffect != null)
    //        fkillerEffect.SetActive(true);
    //    Invoke("DeactivateFkiller", 5f);
    //}

    //void DeactivateFkiller()
    //{
    //    isFkillerActive = false;
    //    if (fkillerEffect != null)
    //        fkillerEffect.SetActive(false);
    //}
}
