using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WhiteLoading_Scene : MonoBehaviour
{
    // 遷移先のシーン名を保持する変数
    public static string NextSceneName;

    public bool _randomFlag;

    public void Start()
    {
        StartCoroutine(LoadScene());
        _randomFlag = false;
    }

    IEnumerator LoadScene()
    {
        // ランダムステージが設定されていれば
        if (NextSceneName == "Random" && !_randomFlag)  
        {
            var _randomStageNum = Random.Range(0, 5);
            NextSceneName = _randomStageNum == 0 ? "Gimmick_Stage_Scene" :
                            _randomStageNum == 1 ? "Ice_Stage_Scene"     :
                            _randomStageNum == 2 ? "Fire_Stage_Scene"    :
                            _randomStageNum == 3 ? "Normal_Stage_Scene"  : "Water_Stage_Scene";

            // フラグ"ON"
            _randomFlag = true;
        }

        if (string.IsNullOrEmpty(NextSceneName))
        {
            Debug.LogError("NextSceneName が設定されていません！");
            yield break;
        }

        AsyncOperation async = SceneManager.LoadSceneAsync(NextSceneName);
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

    // 遷移先シーンを設定する静的メソッド
    public static void SetNextScene(string sceneName)
    {
        NextSceneName = sceneName;
    }
}
