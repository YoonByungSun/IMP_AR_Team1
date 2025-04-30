using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public int life = 3;
    public Image[] hearts; // 하트 이미지 배열
    public GameObject gameOverUI; // Game Over 텍스트 + 버튼들을 담은 부모

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        life--;

        if (life >= 0 && life < hearts.Length)
        {
            hearts[life].enabled = false; // 하트 하나 끄기
        }

        if (life <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
    }
}

