using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhiteLoading_Scene : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("ModeSelect");
        // 自動遷移を無効化
        async.allowSceneActivation = false; 
        while (!async.isDone)
        {

            // ロードが完了したら遷移可能に
            if (async.progress >= 0.9f)
            {
                // 自動で遷移させるなら以下を有効化
                async.allowSceneActivation = true;
            }

            // 次のフレームを待機
            yield return null; 
        }
    }
}
