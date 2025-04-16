using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    // 기존 잘못된 선언
    // public Vector2 planeMin;
    // public Vector2 planeMax;

    // ✅ 수정된 변수 선언 (Vector3 또는 float 사용)
    public float minX = -5f;
    public float maxX = 5f;
    public float minZ = -5f;
    public float maxZ = 5f;


    [Header("Plane 경계")]
    public Vector2 planeMin = new Vector2(-5f, -5f);
    public Vector2 planeMax = new Vector2(5f, 5f);

    public bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool IsInsidePlane(Vector3 pos)
    {
        return pos.x >= minX && pos.x <= maxX &&
               pos.z >= minZ && pos.z <= maxZ;
    }


    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over");

        Time.timeScale = 0f;

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
