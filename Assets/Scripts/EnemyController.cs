using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyType { Mushnub, GreenBlob, AlienBlob }
    public EnemyType enemyType;
    public float speed;
    private Vector3 moveDirection;

    public void InitDir(Vector3 playerPos)
    {
        Vector3 direction = playerPos - transform.position;
        direction.y = 0;
        moveDirection = direction.normalized;
        speed = Random.Range(0.02f, 0.2f);
    }

    void Update()
    {
        if (GameManager.isGameOver || GameManager.isGameClear) return;

        transform.position += moveDirection * speed * Time.deltaTime;
        if (moveDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(moveDirection);

        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        if (Vector3.Distance(transform.position, playerPos) > 5.0f)
            Destroy(gameObject);
    }
}