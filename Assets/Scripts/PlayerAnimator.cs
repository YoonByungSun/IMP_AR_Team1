using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Vector3 lastPosition;
    private Animator animator;

    [Header("Eye GameObjects")]
    public GameObject angry;
    public GameObject panic;
    public GameObject happy;
    public GameObject normal;
    public GameObject surprise;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector3 moveDir = transform.position - lastPosition;
        moveDir.y = 0;

        if (moveDir.sqrMagnitude > 0.00001f)
            transform.forward = moveDir.normalized;

        lastPosition = transform.position;
    }

    public void PlayFly()
    {
        Eye("Normal");
        animator.SetTrigger("FlyTrigger");
    }
    public void PlayCheer()
    {
        Eye("Happy");
        animator.SetTrigger("CheerTrigger");
    }
    public void PlayGetHit()
    {
        Eye("Surprise");
        animator.SetTrigger("GetHitTrigger");
    }
    public void PlayPanic()
    {
        Eye("Panic");
        animator.SetTrigger("PanicTrigger");
    }
    public void PlayShout()
    {
        Eye("Angry");
        animator.SetTrigger("ShoutTrigger");
    }

    private void Eye(string e)
    {
        switch(e)
        {
            case "Angry":
                angry.SetActive(true);
                happy.SetActive(false);
                panic.SetActive(false);
                normal.SetActive(false);
                surprise.SetActive(false);
                break;
            case "Surprise":
                angry.SetActive(false);
                happy.SetActive(false);
                panic.SetActive(false);
                normal.SetActive(false);
                surprise.SetActive(true);
                break;
            case "Normal":
                angry.SetActive(false);
                happy.SetActive(false);
                panic.SetActive(false);
                normal.SetActive(true);
                surprise.SetActive(false);
                break;
            case "Panic":
                angry.SetActive(false);
                happy.SetActive(false);
                panic.SetActive(true);
                normal.SetActive(false);
                surprise.SetActive(false);
                break;
            case "Happy":
                angry.SetActive(false);
                happy.SetActive(true);
                panic.SetActive(false);
                normal.SetActive(false);
                surprise.SetActive(false);
                break;
            default:
                angry.SetActive(false);
                happy.SetActive(false);
                panic.SetActive(false);
                normal.SetActive(true);
                surprise.SetActive(false);
                break;
        }
    }
}
