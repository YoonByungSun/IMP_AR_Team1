using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    public int life = 3;
    public Image[] hearts; // ��Ʈ �̹��� �迭
    public GameObject gameOverUI; // Game Over �ؽ�Ʈ + ��ư���� ���� �θ�

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
            hearts[life].enabled = false; // ��Ʈ �ϳ� ����
        }

        if (life <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // ���� �Ͻ�����
    }
}

