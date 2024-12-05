using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary> ゲーム終了フラグ </summary>
    [ReadOnly]
    public bool gameFinishFLg = false;

    /// <summary> ポーズフラグ </summary>
    [ReadOnly]
    public bool pauseFLg = false;

    // 更新処理
    private void Update()
    {
        // ゲームが終了していたら
        // ゲーム終了処理
        if (gameFinishFLg) { GameFinish(); }

        // ポーズ処理
        Pause();
    }

    /// <summary> ゲーム終了処理 </summary>
    void GameFinish()
    {
        // シーン遷移
        // 現在はゲームを落とす
        Application.Quit();
    }

    // ポーズ(仮)
    void Pause()
    {
        // 仮にPキーでポーズ＆解除
        if (Input.GetKeyDown(KeyCode.P)) { pauseFLg = !pauseFLg; }

        // タイムスケール変更処理
        if(pauseFLg && Time.timeScale != 0f) 
        { 
            Time.timeScale = 0f; 
            Debug.Log("ポーズしました"); 
        }
        if(!pauseFLg && Time.timeScale == 0f) 
        { 
            Time.timeScale = 1f;
            Debug.Log("ポーズ解除");
        }        
    }
}
