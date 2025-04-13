using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerPrefab;
    private GameObject player;

    private float speed = 0.05f;

    private void Start()
    {
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab, transform.position, transform.rotation);
        }
    }

    private void Update()
    {
        if (player != null)
        {
            player.transform.LookAt(transform);

            Vector3 moveDirection = (transform.position - player.transform.position).normalized;
            player.transform.position += moveDirection * speed * Time.deltaTime;
        }
    }
}