using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 2f;
    private Vector3 moveDirection;

    // ���� �� �÷��̾� ��ġ ���� ���� ����
    public void initDir(Vector3 playerPosition)
    {
        moveDirection = (playerPosition - transform.position).normalized;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver) return;

        transform.position += moveDirection * speed * Time.deltaTime;

        // �̵� �������� ȸ�� (���û���)
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}
