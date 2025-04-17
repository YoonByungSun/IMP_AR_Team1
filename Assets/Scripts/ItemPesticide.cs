using UnityEngine;

public class ItemPesticide : MonoBehaviour, ItemInterface
{
    public float radius = 2.0f;

    public void Use(Transform userTransform)
    {
        Collider[] hits = Physics.OverlapSphere(userTransform.position, radius);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Destroy(hit.gameObject);
            }
        }

        Debug.Log("������ ����: �ݰ� " + radius + " ���� �� ����");
    }
}
