using UnityEngine;

public class ItemSpray : MonoBehaviour
{
    public float radius = 2.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (Vector3.Distance(other.transform.position, enemy.transform.position) < radius)
                {
                    Destroy(enemy);
                }
            }
            Destroy(gameObject); 
            AudioManager.Instance.PlaySFX(AudioManager.Instance.spray);
        }
    }
}