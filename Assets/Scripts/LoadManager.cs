using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);     // UI ����
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive); // ���� �� ����
    }
}

