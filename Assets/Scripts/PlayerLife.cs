using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public static PlayerLife Instance;

    public int life = 3;
    private PlayerAnimator animator;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        animator = GetComponent<PlayerAnimator>();
    }

    void Update()
    {
        if (life < 0)
        {
            animator?.PlayPanic();
            GameManager.Instance.GameOver();
        }
    }

    public void TakeDamage()
    {
        life--;
        animator?.PlayGetHit();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.hitSFX);

        if (life >= 0)
        {
            GameObject heart = GameObject.Find("Heart" + (life + 1));
            if (heart != null)
                heart.SetActive(false);
        }
    }
}