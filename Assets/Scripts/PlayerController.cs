using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    private float speed;
    public float health = 100.0f;

    private Animator animator;
    private bool isMoving = false;  // idle 상태 전환용
    private bool isWalking = false; // walk speed == 1.0f
    private bool isFlying = false;  // fly  speed == 1.5f



    // 마커 위 PlayerPrefab 생성
    private void Start()
    {
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, transform.position, transform.rotation);
            animator = GetComponent<Animator>();
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

            // 마커와 일정거리(2.0f) 이상 떨어져 있을 경우 이동속도 증가(1.5배)
            if (distance > 2.0f) speed = 1.5f;
            else speed = 1.0f;

            player.transform.position += moveDirection * speed * Time.deltaTime;


            // 눈 활성화/비활성화
            // angry, normal, surprised, KO, happy






            // 애니메이션 구현

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


        }
    }
}