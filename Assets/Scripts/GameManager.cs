using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool isGameOver = false;
    public static bool isGameClear = false;

    private PlayerAnimator animator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameClear()
    {
        if (isGameClear) return;
        isGameClear = true;
        animator?.PlayCheer();
        UIManager.Instance.SetUI("clear");
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        UIManager.Instance.SetUI("over");
    }

}
