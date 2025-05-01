using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Panels")]
    public GameObject inGameUI;
    public GameObject overUI;
    public GameObject clearUI;
    public GameObject homeUI;  // 타이틀 역할

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject exitButton_GameOver;
    public GameObject returnToTitleButton;
    public GameObject exitButton_Clear;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        SetUI("home");
        if (retryButton != null)
            retryButton.GetComponent<Button>().onClick.AddListener(OnRetryClicked);
        if (exitButton_GameOver != null)
            exitButton_GameOver.GetComponent<Button>().onClick.AddListener(OnExitClicked);
        if (returnToTitleButton != null)
            returnToTitleButton.GetComponent<Button>().onClick.AddListener(OnReturnToTitleClicked);
        if (exitButton_Clear != null)
            exitButton_Clear.GetComponent<Button>().onClick.AddListener(OnExitClicked);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SetUI("inGame");

        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
    }


    //public void ShowGameOverUI()
    //{
    //    SetUI("over");
    //}

    //public void ShowGameClearUI()
    //{
    //    SetUI("clear");
    //}

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        StartCoroutine(RetryGame());
    }


    public void OnReturnToTitleClicked()
    {
        Time.timeScale = 1f;

        if (SceneManager.sceneCount != 1)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name.StartsWith("Stage"))
                    SceneManager.UnloadSceneAsync(scene);
            }
        }

        SetUI("home");
        SceneManager.LoadSceneAsync("Stage1", LoadSceneMode.Additive);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }




    private void RetryGame()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name.StartsWith("Stage"))
            {
                SceneManager.UnloadSceneAsync(scene);
            }
        }
        SetUI("inGame");

        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Single);
        SceneManager.LoadSceneAsync("Stage1", LoadSceneMode.Additive);
    }

    public void SetUI(string name)
    {
        switch (name)
        {
            case "inGame":
                inGameUI.SetActive(true);
                overUI.SetActive(false);
                clearUI.SetActive(false);
                homeUI.SetActive(false);
                break;
            case "over":
                inGameUI.SetActive(false);
                overUI.SetActive(true);
                clearUI.SetActive(false);
                homeUI.SetActive(false);
                break;
            case "clear":
                inGameUI.SetActive(false);
                overUI.SetActive(false);
                clearUI.SetActive(true);
                homeUI.SetActive(false);
                break;
            case "home":
                inGameUI.SetActive(false);
                overUI.SetActive(false);
                clearUI.SetActive(false);
                homeUI.SetActive(true);
                break;
            default:
                Debug.LogError("No UI");
                return;
        }
    }
}
