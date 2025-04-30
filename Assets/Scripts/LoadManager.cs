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
        // 1. UI �� ���� �ε�
        var uiLoad = SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        yield return uiLoad;

        // 2. UI ���� Ȱ�� ������ ���� (�޴� �г��� �⺻���� ���� ���̵���)
        Scene uiScene = SceneManager.GetSceneByName("UI");
        if (uiScene.IsValid())
            SceneManager.SetActiveScene(uiScene);

        // 3. Stage1 ���߿� �ε�
        yield return SceneManager.LoadSceneAsync("Stage1", LoadSceneMode.Additive);
    }
}


