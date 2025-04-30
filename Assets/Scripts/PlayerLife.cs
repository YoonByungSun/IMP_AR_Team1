using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public static PlayerLife Instance;

    public int life = 3;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }


    void Update()
    {
        if (life < 0)
        {
            GameManager.Instance.GameOver(); // ✅ GameOver UI는 GameManager에서 처리
        }
    }

    public void TakeDamage()
    {
        life--;

        if (life >= 0)
        {
            GameObject heart = GameObject.Find("Heart" + (life + 1));
            if (heart != null)
                heart.SetActive(false);
        }
    }
}
