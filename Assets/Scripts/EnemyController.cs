using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Mushnub, GreenBlob, AlienBlob }
    public EnemyType enemyType;
    public float speed;
    private Vector3 moveDirection;

    public void InitDir(Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        direction.y = 0;
        moveDirection = direction.normalized;
        speed = Random.Range(0.01f, 0.1f);
    }

    void Update()
    {
        if (GameManager.isGameOver || GameManager.isGameClear) return;

        transform.position += moveDirection * speed * Time.deltaTime;
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);
    }
}