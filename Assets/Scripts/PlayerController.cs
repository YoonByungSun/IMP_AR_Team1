using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    private float speed;
    public float health = 100.0f;

    private Animator animator;
<<<<<<< HEAD
    private bool isMoving = false;  // idle 상태 전환용
    private bool isWalking = false; // walk speed == 1.0f
    private bool isFlying = false;  // fly  speed == 1.2f
=======
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

>>>>>>> main

    // 마커 위 PlayerPrefab 생성
    private void Start()
    {
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, transform.position, transform.rotation);
<<<<<<< HEAD
            animator = GetComponent<Animator>();
=======
            animator = player.GetComponent<Animator>();
            isWalkingHash = Animator.StringToHash("isWalking");
            isFlyingHash = Animator.StringToHash("isFlying");
            shoutTriggerHash = Animator.StringToHash("shout");
            getHitTriggerHash = Animator.StringToHash("getHit");
            cheerTriggerHash = Animator.StringToHash("cheer");
            dizzyTriggerHash = Animator.StringToHash("dizzy");
            panicTriggerHash = Animator.StringToHash("panic");
>>>>>>> main
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


<<<<<<< HEAD
=======
            animator.SetBool(isWalkingHash, isWalking);
            animator.SetBool(isFlyingHash, isFlying);


>>>>>>> main
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

<<<<<<< HEAD
            

            // 애니메이션 구현========================================

            // 아무런 움직임이 없다면 idle (isMoving = false)
            if (!isMoving)
            {
                // idle 애니메이션 실행
                // animator.SetBool(isIdleHash, true);
            }
            if (isMoving && moveDirection == Vector3.zero)
            {
                // idle 애니메이션 종료
                // animator.SetBool(isIdleHash, false);
            }



            // 이동속도 1.0 pokpok (isMoving = true, isWalking = true)
            if (!isWalking && moveDirection != Vector3.zero)
            {
                // pokpok 애니메이션 실행
                // animator.SetBool(isWalkingHash, true);
            }
            if (isWalking && moveDirection == Vector3.zero)
            {
                // pokpok 애니메이션 종료
                // animator.SetBool(isWalkingHash, false);
            }



            // 이동속도 1.5 fly (isMoving = true, isFlying = true)
            if (!isFlying && moveDirection != Vector3.zero)
            {
                // fly 애니메이션 실행
                // animator.SetBool(isFlyingHash, true);
            }
            if (isFlying && moveDirection == Vector3.zero)
            {
                // fly 애니메이션 종료
                // animator.SetBool(isFlyingHash, false);
            }



            // 공격시 shout (isMoving = true, shout trigger)

            // 체력 감소시 getHit (isMoving = true, getHit trigger)

            // 게임 승리시 cheer (isMoving = true, cheer trigger)

            // 게임 패배시 dizzy (isMoving = true, dizzy trigger)

            // 기타 아이템 획득시 panic (isMoving = true, panic trigger)


=======


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
>>>>>>> main
        }
    }
}