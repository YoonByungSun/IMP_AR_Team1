using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public static PlayerLife Instance;

    public int life = 3;
    private PlayerAnimatorController animatorController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        animatorController = GetComponent<PlayerAnimatorController>();
    }

    void Update()
    {
        if (life < 0)
        {
            animatorController?.PlayPanic();
            GameManager.Instance.GameOver();
        }
    }

    public void TakeDamage()
    {
        life--;
        animatorController?.PlayGetHit();
        AudioManager.Instance.PlaySFX(AudioManager.Instance.hitSFX);

        if (life >= 0)
        {
            GameObject heart = GameObject.Find("Heart" + (life + 1));
            if (heart != null)
                heart.SetActive(false);
        }
    }
}