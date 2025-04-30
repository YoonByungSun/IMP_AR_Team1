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
    public GameObject menuPanel;  // Ÿ��Ʋ ����

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject exitButton_GameOver;
    public GameObject returnToTitleButton;
    public GameObject exitButton_Clear;

    void Start()
    {
        // �ʱ� UI ���� ����
        if (inGameUI != null) inGameUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);  // ���� �� Ÿ��Ʋ ȭ�鸸 ���̵���

        // ��ư �̺�Ʈ ���� (���û���, OnClick���� �����ص� ����)
        if (retryButton != null)
            retryButton.GetComponent<Button>().onClick.AddListener(OnRetryClicked);

        if (exitButton_GameOver != null)
            exitButton_GameOver.GetComponent<Button>().onClick.AddListener(OnExitClicked);

        if (returnToTitleButton != null)
            returnToTitleButton.GetComponent<Button>().onClick.AddListener(OnReturnToTitleClicked);

        if (exitButton_Clear != null)
            exitButton_Clear.GetComponent<Button>().onClick.AddListener(OnExitClicked);

        // PlayerController�� GameOverUI ����
        var player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.gameOverUI = gameOverUI;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        // MenuPanel ����� ���� �� �ε�
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

    // === ��ư�� public �Լ��� (OnClick���� ���� ���� ����) ===

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnReturnToTitleClicked()
    {
        Time.timeScale = 1f;

        // UI ��ȯ
        if (gameClearUI != null) gameClearUI.SetActive(false);
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);

        // ���� ���� �� ��ε� (UI ���� ����)
        var currentScene = SceneManager.GetActiveScene();
        if (currentScene.name != "UI")
        {
            SceneManager.UnloadSceneAsync(currentScene);
        }
    }

    public void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("���� ���� (���忡���� �۵�)");
    }
}
