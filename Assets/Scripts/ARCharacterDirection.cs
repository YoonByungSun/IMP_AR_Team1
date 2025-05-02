using UnityEngine;

public class ARCharacterDirection : MonoBehaviour
{
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 moveDir = transform.position - lastPosition;
        moveDir.y = 0; // 수평 방향만 고려

        // 이동이 있을 때만 즉시 방향 전환
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            transform.forward = moveDir.normalized;
        }

        lastPosition = transform.position;
    }
}
