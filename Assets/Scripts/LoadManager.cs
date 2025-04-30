using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(LoadScenesSequentially());
    }

    IEnumerator LoadScenesSequentially()
    {
        // 1. UI 씬 먼저 로드
        var uiLoad = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        yield return uiLoad;

        // 2. UI 씬을 활성 씬으로 설정 (메뉴 패널이 기본으로 먼저 보이도록)
        Scene uiScene = SceneManager.GetSceneByName("UI");
        if (uiScene.IsValid())
            SceneManager.SetActiveScene(uiScene);

        // 3. Stage1 나중에 로드
        yield return SceneManager.LoadSceneAsync("Stage1", LoadSceneMode.Additive);
    }
}


