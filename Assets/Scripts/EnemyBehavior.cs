using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 moveDirection;

    // 스폰 시 플레이어 위치 기준 방향 설정
    public void initDir(Vector3 playerPosition)
    {
        moveDirection = (playerPosition - transform.position).normalized;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        transform.position += moveDirection * speed * Time.deltaTime;

        // 이동 방향으로 회전 (선택사항)
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}
