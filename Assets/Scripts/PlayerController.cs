using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject gameOverUI;
    public GameObject gameClearUI;
    public GameObject menuPanel;  // 시작 시 타이틀 역할

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject exitButton_GameOver;
    public GameObject returnToTitleButton;
    public GameObject exitButton_Clear;

    void Start()
    {
        // 시작 시 UI 상태 설정
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // 버튼 이벤트 연결
        if (retryButton != null)
            retryButton.GetComponent<Button>().onClick.AddListener(OnRetryClicked);

        if (exitButton_GameOver != null)
            exitButton_GameOver.GetComponent<Button>().onClick.AddListener(OnExitClicked);

        if (returnToTitleButton != null)
            returnToTitleButton.GetComponent<Button>().onClick.AddListener(OnReturnToTitleClicked);

        if (exitButton_Clear != null)
            exitButton_Clear.GetComponent<Button>().onClick.AddListener(OnExitClicked);

        // PlayerController 연결 (게임 오버 시 UI 호출용)
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.gameOverUI = gameOverUI;
        }

        // PlayerLife 연결 (라이프 시스템 게임 오버 UI용)
        var playerLife = FindObjectOfType<PlayerLife>();
        if (playerLife != null)
        {
            playerLife.gameOverUI = gameOverUI;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        // 타이틀 메뉴 숨기고 게임 시작
        if (menuPanel != null)
            menuPanel.SetActive(false);

        // Stage1 씬 Additive 로드
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
    }

    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ShowGameClearUI()
    {
        if (gameClearUI != null)
        {
            gameClearUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;

        // 현재 활성 씬 다시 로드 (UI는 유지됨)
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "UI")
        {
            SceneManager.UnloadSceneAsync(currentScene.name);
            SceneManager.LoadScene(currentScene.name, LoadSceneMode.Additive);
        }
    }

    public void OnReturnToTitleClicked()
    {
        Time.timeScale = 1f;

        // 게임 UI 숨기고 메뉴 다시 표시
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // 현재 게임 씬 언로드 (UI는 남겨둠)
        var currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "UI")
        {
            SceneManager.UnloadSceneAsync(currentScene.name);
        }
    }

    public void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("게임 종료 (빌드 환경에서만 종료됩니다)");
    }
}



