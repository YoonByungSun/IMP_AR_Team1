using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    public int life = 3;
    //public Image[] hearts; // 하트 이미지 배열

    private GameObject[] hearts;

    void Start()
    {
        hearts[0] = GameObject.Find("Heart1");
        hearts[1] = GameObject.Find("Heart2");
        hearts[2] = GameObject.Find("Heart3");
    }

    private void Update()
    {
        if (life < 0) GameManager.Instance.GameOver();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        life--;
        hearts[life].gameObject.SetActive(false);
    }

    //void GameOver()
    //{

    //    GameObject.Find("GameOverUI").SetActive(true);
    //    Time.timeScale = 0f; // 게임 일시정지
    //}
}

