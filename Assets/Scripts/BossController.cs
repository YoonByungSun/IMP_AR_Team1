using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    void Update()
    {
        if (GameManager.isGameOver || GameManager.isGameClear) return;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }
}
