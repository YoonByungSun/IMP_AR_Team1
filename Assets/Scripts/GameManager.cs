using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isGameOver = false;
    public bool isGameClear = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameClear()
    {
        if (isGameClear) return;
        isGameClear = true;
        Debug.Log("Game Clear!");
        UIManager.Instance.SetUI("clear");
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("Game Over");
        UIManager.Instance.SetUI("over");
    }
}
