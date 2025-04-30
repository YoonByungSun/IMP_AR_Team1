using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject inGameUI;
    public GameObject gameOverUI;
    public GameObject gameClearUI;
    public GameObject menuPanel;  // 타이틀 역할

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject exitButton_GameOver;
    public GameObject returnToTitleButton;
    public GameObject exitButton_Clear;

    void Start()
    {
        // 초기 UI 상태 설정
        if (inGameUI != null) inGameUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);  // 시작 시 타이틀 화면만 보이도록

        // 버튼 이벤트 연결 (선택사항, OnClick에서 연결해도 무방)
        if (retryButton != null)
            retryButton.GetComponent<Button>().onClick.AddListener(OnRetryClicked);

        if (exitButton_GameOver != null)
            exitButton_GameOver.GetComponent<Button>().onClick.AddListener(OnExitClicked);

        if (returnToTitleButton != null)
            returnToTitleButton.GetComponent<Button>().onClick.AddListener(OnReturnToTitleClicked);

        if (exitButton_Clear != null)
            exitButton_Clear.GetComponent<Button>().onClick.AddListener(OnExitClicked);

        // PlayerController에 GameOverUI 연결
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.gameOverUI = gameOverUI;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        // MenuPanel 숨기고 게임 씬 로드
        if (menuPanel != null)
            menuPanel.SetActive(false);
        inGameUI.SetActive(true);

        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
    }


    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    public void ShowGameClearUI()
    {
        if (gameClearUI != null)
            gameClearUI.SetActive(true);
    }

    // === 버튼용 public 함수들 (OnClick에서 직접 연결 가능) ===

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToTitleClicked()
    {
        Time.timeScale = 1f;

        // UI 전환
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // 현재 게임 씬 언로드 (UI 씬은 유지)
        var currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "UI")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    public void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("게임 종료 (빌드에서만 작동)");
    }
}
