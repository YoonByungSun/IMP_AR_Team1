using UnityEngine;

public class ItemSpray : ItemGeneric
{
    public float radius = 2.0f;

    private void Reset()
    {
        itemName = "Spray";
    }

    public override void Use(Transform player)
    {
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(player.position, enemy.transform.position) < radius)
            {
                Destroy(enemy);
            }
        }
    }
}
