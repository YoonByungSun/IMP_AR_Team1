using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;


    private float speed;
    public float health = 100.0f;

    private Animator animator;
    private bool isMoving = false;
    private bool isWalking = false;
    private bool isFlying = false;
    private bool isShouting = false;
    private bool isDizzying = false;

    int isWalkingHash;
    int isFlyingHash;
    int shoutTriggerHash;
    int getHitTriggerHash;
    int cheerTriggerHash;
    int dizzyTriggerHash;
    int panicTriggerHash;

    // 마커 위 PlayerPrefab 생성
    private void Start()
    {
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, transform.position, transform.rotation);
            animator = player.GetComponent<Animator>();
            //isWalkingHash = Animator.StringToHash("isWalking");
            isFlyingHash = Animator.StringToHash("isFlying");
            shoutTriggerHash = Animator.StringToHash("shout");
            getHitTriggerHash = Animator.StringToHash("getHit");
            cheerTriggerHash = Animator.StringToHash("cheer");
            dizzyTriggerHash = Animator.StringToHash("dizzy");
            panicTriggerHash = Animator.StringToHash("panic");
        }
    }

    // 마커 위 빈 오브젝트(PlayerIndicator)를 향해 이동 | 마커 위 빈 오브젝트(PlayerIndicator) 생성되도록 해놨음
    // 마커를 닭 모이 같은걸로 바꾸는거 어때요
    private void Update()
    {
        if (player != null)
        {
            // 마커 향해 이동
            player.transform.LookAt(transform);

            Vector3 moveDirection = (transform.position - player.transform.position).normalized;
            float distance = Vector3.Distance(transform.position, player.transform.position);

            // 마커와 일정거리(10.0f) 이상 떨어져 있을 경우 이동속도 증가(1.2배)
            //if (distance > 10.0f) speed = 1.2f;
            //else speed = 1.0f;
            speed = 0.1f;
            player.transform.position += moveDirection * speed * Time.deltaTime;

            //animator.SetBool(isWalkingHash, isWalking);
            animator.SetBool(isFlyingHash, isFlying);

            // 감정별 눈 활성화/비활성화=====================================
            bool isNormal = true;
            bool isAngry = false;
            bool isSurprised = false;
            bool isKO = false;
            bool isHappy = false;
            // angry, normal, surprised, KO, happy
            // Input.GetKey 부분 조건 변경 필요
            if (Input.GetKey("a"))
            {
                isNormal = false;
                isAngry = true;
            }
            if (Input.GetKey("s"))
            {
                isNormal = false;
                isSurprised = true;
            }
            if (Input.GetKey("k"))
            {
                isNormal = false;
                isKO = true;
            }
            if (Input.GetKey("h"))
            {
                isNormal = false;
                isHappy = true;
            }
            if (Input.GetKey("n"))
            {
                isAngry = false;
                isSurprised = false;
                isKO = false;
                isHappy = false;
                isNormal = true;
            }
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

            // 애니메이션 구현========================================
            if (Input.GetKeyDown(KeyCode.Space)) // shout
                animator.SetTrigger(shoutTriggerHash);

            if (Input.GetKeyDown(KeyCode.G)) // getHit
                animator.SetTrigger(getHitTriggerHash);

            if (Input.GetKeyDown(KeyCode.C)) // cheer
                animator.SetTrigger(cheerTriggerHash);

            if (Input.GetKeyDown(KeyCode.D)) // dizzy
                animator.SetTrigger(dizzyTriggerHash);

            if (Input.GetKeyDown(KeyCode.P)) // panic
                animator.SetTrigger(panicTriggerHash);




            // 아이템 사용 =====================================================
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && inventory.Count > 0)
            {
                InventoryItem item = inventory[0];
                inventory.RemoveAt(0);

                GameObject temp = Instantiate(item.itemPrefab, transform.position, Quaternion.identity);
                ItemInterface usable = temp.GetComponent<ItemInterface>();
                if (usable != null)
                {
                    usable.Use(transform);
                }

                Destroy(temp);
            }
        }
    }



    // 아이템 관련 코드 ========================================================
    [System.Serializable]
    public class InventoryItem
    {
        public string itemName;
        public GameObject itemPrefab;
    }

    private List<InventoryItem> inventory = new List<InventoryItem>();

    // 아이템 충돌 시 호출됨
    public void AddItem(string name, GameObject prefab)
    {
        inventory.Add(new InventoryItem { itemName = name, itemPrefab = prefab });
        Debug.Log("New item added in inventory: " + name);
        Debug.Log(inventory.Count + " items in inventory.");
    }
}
