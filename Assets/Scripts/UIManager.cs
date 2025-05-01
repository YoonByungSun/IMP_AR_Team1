using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Lists")]
    public GameObject inGameUI;
    public GameObject overUI;
    public GameObject clearUI;
    public GameObject homeUI;

    [Header("Buttons")]
    public GameObject retryButton;
    public GameObject exitButton_GameOver;
    public GameObject homeButton;
    public GameObject exitButton_Clear;

    [Header("Stage Texts")]
    public Text stage1Text;
    public Text stage2Text;
    public Text stage3Text;
    public Color a = new Color32(0xFF, 0x83, 0x9E, 0xFF);
    public Color b = new Color32(0xFF, 0xFF, 0xFF, 0xFF);

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
        if (homeButton != null)
            homeButton.GetComponent<Button>().onClick.AddListener(OnHomeClicked);
        if (exitButton_Clear != null)
            exitButton_Clear.GetComponent<Button>().onClick.AddListener(OnExitClicked);
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SetUI("inGame");
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
        SetUI("Stage1");
    }

    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        StartCoroutine(RetryGame());
        StartGame();
    }


    public void OnHomeClicked()
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
        SceneManager.LoadSceneAsync("UI", LoadSceneMode.Single);
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private IEnumerator RetryGame()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name.StartsWith("Stage"))
            {
                yield return SceneManager.UnloadSceneAsync(scene);
            }
        }
        yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Single);
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
            case "Stage1":
                stage1Text.color = a;
                stage1Text.fontSize = 55;
                stage2Text.color = b;
                stage2Text.fontSize = 40;
                stage3Text.color = b;
                stage3Text.fontSize = 40;
                break;
            case "Stage2":
                stage1Text.color = b;
                stage1Text.fontSize = 40;
                stage2Text.color = a;
                stage2Text.fontSize = 55;
                stage3Text.color = b;
                stage3Text.fontSize = 40;
                break;
            case "Stage3":
                stage1Text.color = b;
                stage1Text.fontSize = 40;
                stage2Text.color = b;
                stage2Text.fontSize = 40;
                stage3Text.color = a;
                stage3Text.fontSize = 55;
                break;
            default:
                Debug.LogError("No UI");
                return;
        }
    }
}
