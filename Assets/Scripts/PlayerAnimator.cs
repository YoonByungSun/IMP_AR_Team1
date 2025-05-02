using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        lastPosition = transform.position;
    }

    // Look at Moving Direction
    void Update()
    {
        Vector3 moveDir = transform.position - lastPosition;
        moveDir.y = 0;

        if (moveDir.sqrMagnitude > 0.0001f)
        {
            transform.forward = moveDir.normalized;
        }

        lastPosition = transform.position;
    }

    public void PlayFly()
    {
        animator.SetTrigger("FlyTrigger");
    }
    public void PlayCheer()
    {
        animator.SetTrigger("CheerTrigger");
    }
    public void PlayGetHit()
    {
        animator.SetTrigger("GetHitTrigger");
    }
    public void PlayPanic()
    {
        animator.SetTrigger("PanicTrigger");
    }
    public void PlayShout()
    {
        animator.SetTrigger("ShoutTrigger");
    }
}
