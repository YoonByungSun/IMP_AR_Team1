using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClearManager : MonoBehaviour
{
    public GameObject clearUI; // 게임 클리어 UI (버튼 포함된 부모)

    public void GameClear()
    {
        clearUI.SetActive(true);
        Time.timeScale = 0f; // 게임 멈춤
    }

    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title"); // Title 씬 이름 정확히 입력
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
