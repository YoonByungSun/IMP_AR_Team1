using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("BGM")]
    public AudioSource bgmSource;
    public AudioClip stage1BGM;
    public AudioClip stage2BGM;
    public AudioClip stage3BGM;
    public AudioClip stageClearBGM;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip hitSFX;
    public AudioClip growSFX;
    public AudioClip buttonSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이름에 따라 BGM 교체
        switch (scene.name)
        {
            case "Stage1":
                PlayBGM(stage1BGM);
                break;
            case "Stage2":
                PlayBGM(stage2BGM);
                break;
            case "Stage3":
                PlayBGM(stage3BGM);
                break;
            case "GameClear":
                PlayBGM(stageClearBGM);
                break;
            default:
                bgmSource.Stop();
                break;
        }
    }

    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}

