using UnityEngine;

public class ItemController : MonoBehaviour
{
    private void OnTriigerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
            Destroy(gameObject);
        }
    }

}
