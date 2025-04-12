using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // 게임 종료 상태
    public bool isGameOver = false;

    // 적 스폰 범위 (x: 좌우, y: 앞뒤)
    public Vector2 planeMin = new Vector2(-10f, -10f);
    public Vector2 planeMax = new Vector2(10f, 10f);

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // 씬 전환 시에도 유지 (선택사항)
    }

    private void Update()
    {
        // 예시: ESC 키를 누르면 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameOver = true;
        }
    }
}
