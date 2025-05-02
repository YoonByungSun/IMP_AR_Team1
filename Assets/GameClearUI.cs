using UnityEngine;

public class GameClearUI : MonoBehaviour
{
    void OnEnable()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.stageClearBGM);
    }
}
