using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static float scale;

    private int bossKillCount = 0;
    private PlayerAnimator animator;

    void Start()
    {
        animator = GetComponent<PlayerAnimator>();
        StartCoroutine(FlyRoutine());
    }

     IEnumerator FlyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
            animator?.PlayFly();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyController enemy = other.GetComponent<EnemyController>();

            switch (enemy.enemyType)
            {
                case EnemyController.EnemyType.Mushnub:
                    ScaleUp(0.002f);
                    Destroy(other.gameObject);
                    animator?.PlayShout();
                    break;
                case EnemyController.EnemyType.GreenBlob:
                    if (scale >= 0.03f)
                    {
                        ScaleUp(0.002f);
                        Destroy(other.gameObject);
                        animator?.PlayShout();
                    }
                    else PlayerLife.Instance.TakeDamage();
                    break;
                case EnemyController.EnemyType.AlienBlob:
                    if (scale >= 0.06f)
                    {
                        ScaleUp(0.002f);
                        Destroy(other.gameObject);
                        animator?.PlayShout();
                    }
                    else PlayerLife.Instance.TakeDamage();
                    break;
                default:
                    Debug.LogError("Unknown Enemy Type");
                    break;
            }
        }

        if (other.CompareTag("Boss"))
        {
            if (scale >= 0.1f)
            {
                Destroy(other.gameObject);
                bossKillCount++;
                animator?.PlayShout();
                if (bossKillCount >= 2) GameManager.Instance.GameClear();
            }
            else GameManager.Instance.GameOver();
        }
    }

    void ScaleUp(float amount)
    {
        scale += amount;
        transform.localScale = new Vector3(scale, scale, scale);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.growSFX);

        // Change Stage if Scale Value is enough
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
        if (IsSceneLoaded(prevScene))
            yield return SceneManager.UnloadSceneAsync(prevScene);
        if (!IsSceneLoaded(nextScene))
            yield return SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);
    }
}


