using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);     // UI 먼저
        SceneManager.LoadScene("Stage1", LoadSceneMode.Additive); // 게임 씬 나중
    }
}

