using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// シーン管理
using UnityEngine.SceneManagement;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// マウスホバー検知
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{

    // PostProcessing用のVolume
    [SerializeField] Volume     globalVolume;
    // プレイ中のUI
    [SerializeField] GameObject playCanvas;
    // メニューUI
    [SerializeField] GameObject menuCanvas;

    // Tabキーテクスチャ
    [SerializeField] GameObject tabKeyTexture;
    // スタートボタンテクスチャ
    [SerializeField] GameObject startButtonTexture;

    // メニューテクスチャ
    [SerializeField] GameObject menuTexture;

    // ボタンの外枠を管理
    [SerializeField] private GameObject restartButtonOutLine;
    [SerializeField] private GameObject characterSelectButtonOutLine;
    [SerializeField] private GameObject quitButtonOutLine;


    // 全てのボタン
    [SerializeField] private GameObject[] menuButtons;

    // 現在選択中にボタン番号
    private int currentButtonIndex = 0;

    // DepthOfField効果
    private DepthOfField depthOfField;
    // メニューがアクティブか
    private bool isPaused = false;
    // ポーズがアクティブかどうかを外部から読み取り可能に
    public bool IsPaused => isPaused;

    // コントローラーを使用中か
    private bool isUsingController = false;

    // ナビゲーション間の遅延
    private float navigationDelay = 0.2f;
    // 最後にナビゲーションを行った時間
    private float lastNavigationTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // DepthOfFieldエフェクトの取得
        globalVolume.profile.TryGet(out depthOfField);
        if (depthOfField == null)
            Debug.LogError("DepthOfField is not found in the global volume");

        // 初期状態
        SwitchCanvas(false);
        //ShowTabKeyGuide(true);
        // 初期状態から入力デバイスを判定
        DetectInputDevice();

        SetButtonOutLineActive(false);
    }

    private void Update()
    {
        // 常に入力デバイスの判定を取る
        DetectInputDevice();

        // キーボードまたはコントローラーの入力でポーズを切り替え
        if(!isPaused && 
            (Input.GetKeyDown(KeyCode.Tab)||Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            SwitchCanvas(true);
            NavigateWithGamePad();
        }
        else if(isPaused &&
            (Input.GetKeyDown(KeyCode.Tab)||Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            SwitchCanvas(false);
        }

        // ゲームパッドでのナビゲーション処理
        if(isPaused && isUsingController)
        {
            NavigateWithGamePad();
        }


        //// Tabキーでポーズを切り替え
        //if (Input.GetKeyDown(KeyCode.Tab) && !isPaused)
        //{
        //    SwitchCanvas(true);
        //}
    }


    // 入力デバイスを判定しUIを切り替える
    void DetectInputDevice()
    {
        // コントローラーが接続されているか確認
        string[] joySticks = Input.GetJoystickNames();
        bool controllerConnected = joySticks.Length > 0 && !string.IsNullOrEmpty(joySticks[0]);

        if(controllerConnected != isUsingController)
        {
            isUsingController = controllerConnected;
            // 画像切り替え
            SwitchUI();
        }
    }

    // キーボードUIとコントローラーUIを切り替える
    void SwitchUI()
    {
        tabKeyTexture.SetActive(!isUsingController);
        startButtonTexture.SetActive(isUsingController);
    }


    // キャンバスの切り替え
    public void SwitchCanvas(bool showMenu)
    {
        isPaused = showMenu;

        playCanvas.SetActive(!showMenu);
        menuCanvas.SetActive(showMenu);
        SwitchDepthOfField(showMenu);

        // ポーズ中は時間を停止
        Time.timeScale = showMenu ? 0f : 1f;

        if(isPaused)
        {
            // 初期ボタンを選択
            currentButtonIndex = 0;
            UpdateOutLinePosition();
        }
        else
        {
            SetButtonOutLineActive(false);
        }

        // TabキーUI非表示
        //ShowTabKeyGuide(!showMenu);
    }


    private void UpdateOutLinePosition()
    {
        // 全アウトライン非表示
        SetButtonOutLineActive(false);

        if(menuButtons[currentButtonIndex] == menuButtons[0])
        {
            restartButtonOutLine.SetActive(true);
        }
        else if(menuButtons[currentButtonIndex] == menuButtons[1])
        {
            characterSelectButtonOutLine.SetActive(true);
        }
        else if(menuButtons[currentButtonIndex] == menuButtons[2])
        {
            quitButtonOutLine.SetActive(true);
        }
    }

    private void NavigateWithGamePad()
    {
        float verticalInput = Input.GetAxis("Vertical");

        // 一定時間が経過していない場合は処理を行わない
        if (Time.unscaledTime - lastNavigationTime < navigationDelay)
            return;

        // 下に移動
        if (verticalInput < -0.5f)
        {
            currentButtonIndex = (currentButtonIndex + 1) % menuButtons.Length;
            UpdateOutLinePosition();
            // 最後のナビゲーション時間を更新
            lastNavigationTime = Time.unscaledTime;
        }
        // 上に移動
        else if(verticalInput > 0.5f)
        {
            currentButtonIndex = (currentButtonIndex - 1 + menuButtons.Length) % menuButtons.Length;
            UpdateOutLinePosition();
            // 最後のナビゲーション時間を更新
            lastNavigationTime = Time.unscaledTime;
        }

        // 決定が押された時
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            ExecuteEvents.Execute(menuButtons[currentButtonIndex], new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }

    public void SwitchDepthOfField(bool _switch)
    {
        if(depthOfField != null)
        {
            depthOfField.active = _switch;
        }
    }

    // Tabキー画像の表示切替
    private void ShowTabKeyGuide(bool show)
    {
        if(tabKeyTexture != null)
        {
            tabKeyTexture.SetActive(show);
        }
        if(menuTexture != null)
        {
            menuTexture.SetActive(show);
        }
    }

    // ボタンが押された時に呼び出される関数
    public void OnRestartButtonPressed()
    {
        SwitchCanvas(false);
    }

    public void OnCharacterSelectButtonPressed()
    {
        // キャラクター選択シーンへ移動
        Time.timeScale = 1f;
        //SceneManager.LoadScene("CustomizeScene");
    }

    public void OnQuitButtonPressed()
    {
        //Debug.Log("Game Quit");
        Application.Quit();
    }


    // ボタン外枠を切り替える
    private void SetButtonOutLineActive(bool active)
    {
        if (restartButtonOutLine != null) restartButtonOutLine.SetActive(active);
        if (characterSelectButtonOutLine != null) characterSelectButtonOutLine.SetActive(active);
        if (quitButtonOutLine != null) quitButtonOutLine.SetActive(active);
    }

    // ボタンのホバーイベント
    public void OnRestartButtonHoverEnter() => restartButtonOutLine.SetActive(true);
    public void OnRestartButtonHoverExit() => restartButtonOutLine.SetActive(false);

    public void OnCharacterSelectButtonHoverEnter() => characterSelectButtonOutLine.SetActive(true);
    public void OnCharacterSelectButtonHoverExit() => characterSelectButtonOutLine.SetActive(false);

    public void OnQuitButtonHoverEnter() => quitButtonOutLine.SetActive(true);
    public void OnQuitButtonHoverExit() => quitButtonOutLine.SetActive(false);
}
