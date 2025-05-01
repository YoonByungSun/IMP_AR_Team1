using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GameClear()
    {
        Debug.Log("Game Clear!");
        UIManager.Instance.SetUI("clear");
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        UIManager.Instance.SetUI("over");
    }
}
