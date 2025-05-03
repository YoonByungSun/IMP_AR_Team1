using UnityEngine;

public class ItemSpray : MonoBehaviour
{
    public float radius = 2.0f;
    private Vector3 rotateSpeed = new Vector3(45f, 90f, 30f);

    private void Update()
    {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector3.Distance(other.transform.position, enemy.transform.position) < radius)
                    Destroy(enemy);
            }
            Destroy(gameObject); 
            AudioManager.Instance.PlaySFX(AudioManager.Instance.spray);
        }
    }
}