using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ���� ���� ����
    public bool isGameOver = false;

    // �� ���� ���� (x: �¿�, y: �յ�)
    public Vector2 planeMin = new Vector2(-10f, -10f);
    public Vector2 planeMax = new Vector2(10f, 10f);

    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // �� ��ȯ �ÿ��� ���� (���û���)
    }

    private void Update()
    {
        // ����: ESC Ű�� ������ ���� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isGameOver = true;
        }
    }
}
