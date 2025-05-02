using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Function: Manage all UIs except InventoryUI
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Lists")]
    public GameObject inGameUI;
    public GameObject overUI;
    public GameObject clearUI;
    public GameObject homeUI;

    [Header("Check Ready")]
    public GameObject waitForPlane;
    public GameObject touchToStart;
    public GameObject waitForPlayer;

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

    // Singletone
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // Init UI
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

    // Check if AR Plane detected, if Room Object created
    // if both is true, call StartGame()
    public void ReadyGame()
    {
        Time.timeScale = 1f;
        SetUI("inGame");
        SetUI("ready");

        StartCoroutine(CheckPlane());
    }

    private IEnumerator CheckPlane()
    {
        while (true)
        {
            bool planeReady = RoomSpawner.Instance.GetPlaneCount() > 0;
            bool roomReady = RoomSpawner.Instance.isSpawned;

            if (!planeReady) // Waiting for Plane Detection...
            {
                waitForPlane.SetActive(true);
                touchToStart.SetActive(false);
            }
            else if (!roomReady) // Touch Any Plane to Get Ready
            {
                waitForPlane.SetActive(false);
                touchToStart.SetActive(true);
            }
            else // Start Game
            {
                waitForPlane.SetActive(false);
                touchToStart.SetActive(false);
                //StartGame();
                StartCoroutine(CheckPlayer());
                yield break;
            }

            yield return new WaitForSeconds(0.3f);
        }
    }

    private IEnumerator CheckPlayer()
    {
        while (true)
        {
            bool playerReady = GameObject.Find("Player") != null;
            if (!playerReady)
                waitForPlayer.SetActive(true);
            else
            {
                waitForPlayer.SetActive(false);
                StartGame();
                yield break;
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    // Call if Plane detected, and Room created
    public void StartGame()
    {
        if (SceneManager.sceneCount == 1){
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSFX);
            SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
        }
        else return;

        SetUI("Stage1");
    }


    public void OnRetryClicked()
    {
        Time.timeScale = 1f;
        StartCoroutine(RetryGame());
        ReadyGame();
    }

    // UI Button events
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

    // Call when UI update needed
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
            case "ready":
                stage1Text.color = b;
                stage1Text.fontSize = 40;
                stage2Text.color = b;
                stage2Text.fontSize = 40;
                stage3Text.color = b;
                stage3Text.fontSize = 40;
                break;
            default:
                Debug.LogError("No UI");
                return;
        }
    }
}
