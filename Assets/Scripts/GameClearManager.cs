using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public GameObject clearUI; // ���� Ŭ���� UI (��ư ���Ե� �θ�)

    public void GameClear()
    {
        clearUI.SetActive(true);
        Time.timeScale = 0f; // ���� ����
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title"); // Title �� �̸� ��Ȯ�� �Է�
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
