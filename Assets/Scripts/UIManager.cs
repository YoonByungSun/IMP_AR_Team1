using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("Check Ready")]
    public GameObject waitForPlane;
    public GameObject touchToStart;
    public GameObject waitForPlayer;

    [Header("Buttons")]
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

    // Init UI
    void Start()
    {
        SetUI("home");
        if (exitButton_GameOver != null)
            exitButton_GameOver.GetComponent<Button>().onClick.AddListener(OnExitClicked);
        if (homeButton != null)
            homeButton.GetComponent<Button>().onClick.AddListener(OnHomeClicked);
        if (exitButton_Clear != null)
            exitButton_Clear.GetComponent<Button>().onClick.AddListener(OnExitClicked);
    }

    // Check if AR Plane detected, Room Object created and Player Object Spawned
    // if all of them are true, call StartGame()
    public void ReadyGame()
    {
        GameManager.isGameOver = false;
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

    // Call if Plane detected, Room Created and Player Spawned
    public void StartGame()
    {
        if (SceneManager.sceneCount == 1)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.buttonSFX);
            SceneManager.LoadScene("Stage1", LoadSceneMode.Additive);
        }
        else return;

        SetUI("Stage1");
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
        SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    public void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
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

    public bool IsOverUI(Vector2 pos)
    {
        if (homeUI.active)
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = new Vector2(pos.x, pos.y);

            // make a raycast from the pos to check if the raycast hits any UI elements
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            return results.Count > 0;
        }
        return false;
    }
}
