using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayFly() => animator.SetTrigger("FlyTrigger");
    public void PlayCheer() => animator.SetTrigger("CheerTrigger");
    public void PlayGetHit() => animator.SetTrigger("GetHitTrigger");
    public void PlayPanic() => animator.SetTrigger("PanicTrigger");
    public void PlayShout() => animator.SetTrigger("ShoutTrigger");
}
