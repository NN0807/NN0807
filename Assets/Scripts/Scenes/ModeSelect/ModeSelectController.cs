using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ModeSelectController : MonoBehaviour
{

    // 各ボタン
    public Button battleModeButton;
    public Button onlineModeButton;
    public Button tutorialModeButton;
    private Button[] buttons;
    // 選択中のボタン番号
    private int selectedIndex = -1;

    // ゲームパッド
    private Gamepad gamepad;

    // 入力猶予時間(秒
    private float inputDelay = 0.3f;
    // 最後に入力を受け付けた時間
    private float lastInputTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // ボタン配列を初期化
        buttons = new Button[] { battleModeButton, onlineModeButton, tutorialModeButton };

        // モードセレクトBGM
        StartCoroutine(AudioManager.instance.StartFuncPlay(BGMPath.ModeSelectBGM, 0.004f, 0f, 1f, true));
    }

    // Update is called once per frame
    void Update()
    {
        // コントローラー接続確認
        gamepad = Gamepad.current;

        // ゲームパッドの接続がなければ処理しない
        if (gamepad == null)
            return;

        Vector2 stickInput = gamepad.leftStick.ReadValue();

        // 初期状態からスティック入力があったらソロモードを選択
        if(selectedIndex == -1 && stickInput.magnitude > 0.1f)
        {
            // ソロモードを選択
            SelectButton(0);
            // ディレイをリセット
            lastInputTime = Time.time;
        }

        // Lスティックで選択移動
        if (selectedIndex != -1 && Time.time - lastInputTime >= inputDelay)
        {
            // 右方向
            if (stickInput.x > 0.5f)
            {
                // 次のボタンを選択
                ChangeSelection(1);
                // 入力時間を記録
                lastInputTime = Time.time;
            }
            // 左方向
            else if (stickInput.x < -0.5f)
            {
                // 前のボタンを選択
                ChangeSelection(-1);
                // 入力時間を記録
                lastInputTime = Time.time;
            }
        }

        // Aボタンでシーン遷移
        if(selectedIndex != -1 && gamepad.buttonSouth.wasPressedThisFrame)
        {
            ActiveButton();
        }
    }

    // 選択ボタンの分岐
    private void SelectButton(int index)
    {
        // 現在のボタン選択を解除
        if(selectedIndex >= 0 && selectedIndex < buttons.Length)
        {
            ExecuteEvents.Execute(buttons[selectedIndex].gameObject,new PointerEventData(EventSystem.current),ExecuteEvents.pointerExitHandler);
        }

        // 新しいボタンを選択
        selectedIndex = Mathf.Clamp(index, 0, buttons.Length - 1);
        // マウスが乗った時と同じ処理を実行
        ExecuteEvents.Execute(buttons[selectedIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerEnterHandler);
    }

    // 選択中のボタンを変更
    private void ChangeSelection(int direction)
    {
        int newIndex = selectedIndex + direction;

        // 番号が範囲外の場合は無視
        if (newIndex < 0 || newIndex >= buttons.Length)
            return;

        // 現在のボタン選択を解除
        ExecuteEvents.Execute(buttons[selectedIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerExitHandler);

        // 新しいボタンを選択
        SelectButton(newIndex);

        // 選択
        AudioManager.instance.Play(SEPath.MoveCursor, 0.004f);
    }

    // 各ボタンの機能
    private void ActiveButton()
    {
        // 現在選択されているボタンをクリック
        ExecuteEvents.Execute(buttons[selectedIndex].gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);

        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, 0.004f);

        // シーン遷移
        switch (selectedIndex)
        {
            // ソロモード
            case 0:
                SceneManager.LoadScene("CustomizeScene");
                break;
            // オンラインモード
            case 1:
                // SceneManager.LoadScene("OnlineModeScene");
                break;
            // チュートリアルモード
            case 2:
                // SceneManager.LoadScene("TutorialModeScene");
                break;
        }
    }
}
