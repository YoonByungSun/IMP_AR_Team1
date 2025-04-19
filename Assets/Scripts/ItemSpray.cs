using UnityEngine;

public class ItemSpray : MonoBehaviour, ItemInterface
{
    public float radius = 2.0f;

    public void Use(Transform player)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(player.position, enemy.transform.position) < radius)
            {
                Destroy(enemy);
            }
        }

        Debug.Log("Pesticide Used.");
    }
}
