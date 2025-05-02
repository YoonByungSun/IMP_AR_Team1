using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float scale = 1f;

    public GameObject fkillerEffect;
    public GameObject gameOverUI;  // GameOver UI 오브젝트 연결용

    private int bossKillCount = 0;
    private PlayerAnimatorController animatorController;

    void Start()
    {
        animatorController = GetComponent<PlayerAnimatorController>();
        StartCoroutine(FlyRoutine());

        string currentScene = SceneManager.GetActiveScene().name;

        // Stage1이 아닐 때만 저장된 스케일 적용
        if (currentScene != "Stage1" && PlayerData.Instance != null)
        {
            scale = PlayerData.Instance.savedScale;
            transform.localScale = new Vector3(scale, scale, scale);
            Debug.Log($"savedScale 적용됨 = {scale}");
        }
    }

     IEnumerator FlyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            animatorController?.PlayFly();
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
                    Debug.Log("Mushnub과 충돌 → ScaleUp");
                    ScaleUp(0.002f);
                    animatorController?.PlayShout();
                    Destroy(other.gameObject);
                    break;

                case EnemyController.EnemyType.GreenBlob:
                case EnemyController.EnemyType.AlienBlob:
                    if (scale >= (enemy.enemyType == EnemyController.EnemyType.GreenBlob ? 0.03f : 0.06f))
                    {
                        ScaleUp(0.002f);
                        Destroy(other.gameObject);
                        animatorController?.PlayShout(); // 적 처치 시
                    }
                    else
                    {
                        PlayerLife.Instance.TakeDamage();
                        // 피격 애니메이션은 PlayerLife에서 실행
                    }
                    break;

                default:
                    Debug.LogError("알 수 없는 enemyType");
                    break;
            }
        }

        if (other.CompareTag("Boss"))
        {
            if (scale >= 0.1f)
            {
                Destroy(other.gameObject);
                bossKillCount++;
                animatorController?.PlayShout(); // 보스 처치 시
                if (bossKillCount >= 2) {GameManager.Instance.GameClear();}
            }
            else
                GameManager.Instance.GameOver();
        }
    }

    void ScaleUp(float amount)
    {
        scale = Mathf.Min(scale + amount, 1.0f);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

        if (PlayerData.Instance != null)
        {
            PlayerData.Instance.savedScale = scale;
            Debug.Log($"스케일 저장됨: {scale}");
        }

        // 스테이지 전환 조건 체크 및 처리
        if (scale >= 0.03f && IsSceneLoaded("Stage1"))
        {
            StartCoroutine(SwitchStage("Stage1", "Stage2"));
            UIManager.Instance.SetUI("Stage2");
        }
        else if (scale >= 0.06f && IsSceneLoaded("Stage2"))
        {
            StartCoroutine(SwitchStage("Stage2", "Stage3"));
            UIManager.Instance.SetUI("Stage3");
        }
    }


    //void GameOver()
    //{
    //    if (isDead) return;
    //    isDead = true;

    //    if (gameOverUI != null)
    //    {
    //        gameOverUI.SetActive(true);
    //        Time.timeScale = 0f;  // 게임 일시정지
    //    }
    //    else
    //    {
    //        Debug.LogWarning("gameOverUI가 연결되지 않았습니다.");
    //    }
    //}
    private bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if (SceneManager.GetSceneAt(i).name == sceneName)
                return true;
        }
        return false;
    }

    private IEnumerator SwitchStage(string prevScene, string nextScene)
    {
        // 이전 스테이지 언로드
        if (IsSceneLoaded(prevScene))
        {
            Debug.Log($"[Stage 전환] {prevScene} 언로드");
            yield return SceneManager.UnloadSceneAsync(prevScene);
        }

        // 다음 스테이지 로드
        if (!IsSceneLoaded(nextScene))
        {
            Debug.Log($"[Stage 전환] {nextScene} 로드");
            yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
        }
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


