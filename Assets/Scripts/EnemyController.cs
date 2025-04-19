using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Mushnub, GreenBlob, AlienBlob }
    public EnemyType enemyType;
    public float speed = 2f;
    private Vector3 moveDirection;

    public void SetInitialDirection(Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        direction.y = 0;
        moveDirection = direction.normalized;
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;

        transform.position += moveDirection * speed * Time.deltaTime;
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}