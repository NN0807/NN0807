using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GameManager : MonoBehaviour
{
    /// <summary> ゲーム終了フラグ </summary>
    [ReadOnly]
    public bool gameFinishFLg = false;

    /// <summary> ポーズフラグ </summary>
    [ReadOnly]
    public bool pauseFLg = false;

    // 入力処理
    [SerializeField]
    private HakopanControls _InputActions;

    /// <summary> キャラクターズ </summary>
    [SerializeField]
    //public CharacterManager[] characterManagers = new CharacterManager[CharacterConst.CHARACTER_NUM];

    private void Start()
    {
        _InputActions = new HakopanControls();
        _InputActions.Enable();
    }

    // 更新処理
    private void Update()
    {
        // ゲームが終了していたら
        // ゲーム終了処理
        if (gameFinishFLg) { GameFinish(); }

        // ポーズ処理
        //Pause();
    }

    /// <summary> ゲーム終了処理 </summary>
    void GameFinish()
    {
        // シーン遷移
        // 現在はゲームを落とす
        Application.Quit();
    }

    // ポーズ(仮)
    //void Pause()
    //{
    //    // 仮にPキーでポーズ＆解除
    //    if (_InputActions.Player.Pause.triggered) { pauseFLg = !pauseFLg; }

    //    // タイムスケール変更処理
    //    if(pauseFLg && Time.timeScale != 0f) 
    //    { 
    //        Time.timeScale = 0f; 
    //        Debug.Log("ポーズしました"); 
    //    }
    //    if(!pauseFLg && Time.timeScale == 0f) 
    //    { 
    //        Time.timeScale = 1f;
    //        Debug.Log("ポーズ解除");
    //    }        
    //}
}
