using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    public float scale = 1f;
    public Vector3 defaultScale = new Vector3(0.01f, 0.01f, 0.01f);
    public GameObject fkillerEffect;
    public float health = 100.0f;

    private float speed;
    private bool isFkillerActive = false;
    private bool isDead = false;
    private int bossKillCount = 0;

    private Animator animator;
    private bool isFlying = false;

    int isFlyingHash;
    int shoutTriggerHash;
    int getHitTriggerHash;
    int cheerTriggerHash;
    int dizzyTriggerHash;
    int panicTriggerHash;

    void Start()
    {
        // 씬 이름 확인 (스케일 저장 기능)
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Stage1" && PlayerData.Instance != null)
        {
            scale = PlayerData.Instance.savedScale;
            transform.localScale = new Vector3(scale, scale, scale);
            Debug.Log($"📌 PlayerController: savedScale 적용됨 = {scale}");
        }

        // PlayerPrefab 인스턴스 및 애니메이터 설정
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, transform.position, transform.rotation);
            animator = player.GetComponent<Animator>();
            isFlyingHash = Animator.StringToHash("isFlying");
            shoutTriggerHash = Animator.StringToHash("shout");
            getHitTriggerHash = Animator.StringToHash("getHit");
            cheerTriggerHash = Animator.StringToHash("cheer");
            dizzyTriggerHash = Animator.StringToHash("dizzy");
            panicTriggerHash = Animator.StringToHash("panic");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어 이동 및 시선
            player.transform.LookAt(transform);
            Vector3 moveDirection = (transform.position - player.transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            speed = 0.1f;
            player.transform.position += moveDirection * speed * Time.deltaTime;

            // 애니메이션
            animator.SetBool(isFlyingHash, isFlying);

            // 감정별 눈 상태 (예시, 조건은 상황 맞게 수정)
            bool isNormal = true;
            bool isAngry = false;
            bool isSurprised = false;
            bool isKO = false;
            bool isHappy = false;
            if (Input.GetKey("a")) { isNormal = false; isAngry = true; }
            if (Input.GetKey("s")) { isNormal = false; isSurprised = true; }
            if (Input.GetKey("k")) { isNormal = false; isKO = true; }
            if (Input.GetKey("h")) { isNormal = false; isHappy = true; }
            if (Input.GetKey("n")) { isAngry = false; isSurprised = false; isKO = false; isHappy = false; isNormal = true; }
            player.transform.Find("rudy_eye_angry_left").gameObject.SetActive(isAngry);
            player.transform.Find("rudy_eye_angry_right").gameObject.SetActive(isAngry);
            player.transform.Find("rudy_eye_surprise_left").gameObject.SetActive(isSurprised);
            player.transform.Find("rudy_eye_surprise_right").gameObject.SetActive(isSurprised);
            player.transform.Find("rudy_eye_KO_left").gameObject.SetActive(isKO);
            player.transform.Find("rudy_eye_KO_right").gameObject.SetActive(isKO);
            player.transform.Find("rudy_eye_happy_left").gameObject.SetActive(isHappy);
            player.transform.Find("rudy_eye_happy_right").gameObject.SetActive(isHappy);
            player.transform.Find("rudy_eye_left").gameObject.SetActive(isNormal);
            player.transform.Find("rudy_eye_right").gameObject.SetActive(isNormal);

            // 애니메이션 트리거
            if (Input.GetKeyDown(KeyCode.Space)) animator.SetTrigger(shoutTriggerHash);
            if (Input.GetKeyDown(KeyCode.G)) animator.SetTrigger(getHitTriggerHash);
            if (Input.GetKeyDown(KeyCode.C)) animator.SetTrigger(cheerTriggerHash);
            if (Input.GetKeyDown(KeyCode.D)) animator.SetTrigger(dizzyTriggerHash);
            if (Input.GetKeyDown(KeyCode.P)) animator.SetTrigger(panicTriggerHash);

            // 아이템 사용 (Inventory.Instance.items를 이용, 예: 첫 번째 아이템 사용)
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began &&
                Inventory.Instance != null && Inventory.Instance.items.Count > 0)
            {
                string itemName = Inventory.Instance.items[0];
                Inventory.Instance.RemoveItem(itemName);
                Debug.Log($"{itemName} 아이템 사용됨");
                // 실제 사용 효과는 아이템 종류에 따라 별도 구현
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[충돌 발생] other.name = {other.name}, tag = {other.tag}");

        // 적과 충돌
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

        // 보스와 충돌
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

        // 아이템 획득 코드 ItemGeneric.cs에 있음
    }

    void ScaleUp(float amount)
    {
        scale = Mathf.Min(scale + amount, 1.0f);
        transform.localScale = new Vector3(scale, scale, scale);
        transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);

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
