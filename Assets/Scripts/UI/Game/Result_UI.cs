using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI判定に必要
using UnityEngine.UI;           // Buttonコンポーネントに必要
using UnityEngine.SceneManagement;

public class Result_UI : MonoBehaviour
{
    // 入力マネージャー
    [SerializeField]
    public InputManager _inputManager;

    // 選択ボタンオブジェクト
    public GameObject   _buttonToSelect;

    // アタッチするボタン
    // ※"1がリトライ" "2がモード選択" "3がタイトルへ"
    public Button[] _resultButtons = new Button[3];

    // 画像
    // ※"1がリトライ" "2がモード選択" "3がタイトルへ"
    public Image[] _images = new Image[3];

    // キーマウ用のボタン画像
    public Sprite[] _resultButtonSprites  = new Sprite[3];

    // コントローラー用のボタン画像
    public Sprite[] _resultAButtonSprites = new Sprite[3];

    // 遷移時の白画像
    public Image _whiteImage;

    // シーン遷移先を選択する時用の黒画像
    public Image _blackImage;

    // イージング時間
    [SerializeField]
    private float _easingTime = 0.3f;

    // 遷移時の白画像の透明値
    private Color _whiteColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    // 画像の開始と終了の拡縮値
    private Vector3  _startScale = new Vector3(540.0f, 118.0f, 1.0f);
    private Vector3 _targetScale = new Vector3(720.0f, 153.0f, 1.0f);

    // コルーチンフラグ
    private bool[] _coroutineFlags  = new bool[3];
    private bool[] _coroutineFlags2 = new bool[3];

    // 各3項目用のタイマーとフラグ
    private float[] _resultTimers = new float[3];
    private bool[]  _resultFlags  = new bool[3];

    // Start is called before the first frame update
    void Start()
    {
        // 最初にボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(_buttonToSelect);

        for (int Index = 0; Index < 3; Index++)
        {
            // 初期設定
            _images[Index].rectTransform.sizeDelta = _startScale;
            _resultTimers[Index] = 0.0f;
            _resultFlags[Index]  = false;
        }

        // 遷移時の白画像の透明値を初期化
        _whiteColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        _whiteImage.color = _whiteColor;

        // ボタンにクリックイベントを登録
        if (_resultButtons[0] != null) _resultButtons[0].onClick.AddListener(OnRetryButtonClick);
        if (_resultButtons[1] != null) _resultButtons[1].onClick.AddListener(OnModeSelectButtonClick);
        if (_resultButtons[2] != null) _resultButtons[2].onClick.AddListener(OnTitleBackButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        // ゲーム終了してから"5秒後"
        // ※プレイヤーが何か入力したらにする予定、、
        if (GameManager.Instance.GetGameFinishTimer() >= 5.0f)  
        {
            for (int Index = 0; Index < 3; Index++)
                _resultButtons[Index].gameObject.SetActive(true);

            // 黒画像表示
            _blackImage.gameObject.SetActive(true);
        }
        else
        {
            for (int Index = 0; Index < 3; Index++)
                _resultButtons[Index].gameObject.SetActive(false);

            // 黒画像表示
            _blackImage.gameObject.SetActive(false);
        }

        // 現在の入力デバイスがゲームパッドであれば
        if (_inputManager.GetCurrentInputDevice() == "Gamepad")
        {
            // ゲームパッド用の画像に差替える
            for (int Index = 0; Index < 3; Index++) 
                _images[Index].sprite = _resultButtonSprites[Index];

            // 現在選択されているGameObjectを取得
            _buttonToSelect = EventSystem.current.currentSelectedGameObject;

            // 選択ボタンが有り、名前が"Retry Button"であれば
            if (_buttonToSelect != null && _buttonToSelect.name == "Retry Button")
            {
                if (!_coroutineFlags[0])
                {
                    StartCoroutine(
                    Scale(_images[0], _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[0]  = true;
                    _coroutineFlags2[0] = false;
                }
            }
            else
            {
                if (!_coroutineFlags2[0])
                {
                    StartCoroutine(
                    Scale(_images[0], _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[0]  = false;
                    _coroutineFlags2[0] = true;
                }
            }
            // 選択ボタンが有り、名前が"ModeSelect Button"であれば
            if (_buttonToSelect != null && _buttonToSelect.name == "ModeSelect Button")
            {
                if (!_coroutineFlags[1])
                {
                    StartCoroutine(
                    Scale(_images[1], _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[1]  = true;
                    _coroutineFlags2[1] = false;
                }
            }
            else
            {
                if (!_coroutineFlags2[1])
                {
                    StartCoroutine(
                    Scale(_images[1], _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[1]  = false;
                    _coroutineFlags2[1] = true;
                }
            }
            // 選択ボタンが有り、名前が"TitleBack Button"であれば
            if (_buttonToSelect != null && _buttonToSelect.name == "TitleBack Button")
            {
                if (!_coroutineFlags[2])
                {
                    StartCoroutine(
                    Scale(_images[2], _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[2]  = true;
                    _coroutineFlags2[2] = false;
                }
            }
            else
            {
                if (!_coroutineFlags2[2])
                {
                    StartCoroutine(
                    Scale(_images[2], _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[2]  = false;
                    _coroutineFlags2[2] = true;
                }
            }
        }
        // 現在の入力デバイスがキーマウであれば
        else if (_inputManager.GetCurrentInputDevice() == "Keyboard/Mouse")
        {
            // キーマウ用の画像に差替える
            for (int Index = 0; Index < 3; Index++)
                _images[Index].sprite = _resultAButtonSprites[Index];

            // マウスの位置を取得する
            Vector3 mousePos = Input.mousePosition;

            // ※レイキャストで取得したかったけど、
            // 全然無理やったんで脳筋失礼します。
            if (mousePos.x >= 690.0f && mousePos.x <= 1230.0f &&
                mousePos.y >= 701.0f && mousePos.y <=  819.0f)
            {
                Debug.Log("リトライボタンに重なっています！");

                if (!_coroutineFlags[0])
                {
                    // カーソル移動音
                    AudioManager.instance.Play(SEPath.MoveCursor, AudioManager.ALL_VOLUME_VALUE);

                    StartCoroutine(
                    Scale(_images[0], _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[0]  = true;
                    _coroutineFlags2[0] = false;
                }
            }
            // リトライボタンボタンが選ばれていなければ
            else
            {
                if (!_coroutineFlags2[0])
                {
                    StartCoroutine(
                    Scale(_images[0], _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[0]  = false;
                    _coroutineFlags2[0] = true;
                }
            }

            // ※レイキャストで取得したかったけど、
            // 全然無理やったんで脳筋失礼します。
            if (mousePos.x >= 690.0f && mousePos.x <= 1230.0f &&
                mousePos.y >= 481.0f && mousePos.y <=  599.0f)
            {
                Debug.Log("モード選択ボタンに重なっています！");

                if (!_coroutineFlags[1])
                {
                    // カーソル移動音
                    AudioManager.instance.Play(SEPath.MoveCursor, AudioManager.ALL_VOLUME_VALUE);

                    StartCoroutine(
                    Scale(_images[1], _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[1]  = true;
                    _coroutineFlags2[1] = false;
                }
            }
            // モード選択ボタンが選ばれていなければ
            else
            {
                if (!_coroutineFlags2[1])
                {
                    StartCoroutine(
                    Scale(_images[1], _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[1]  = false;
                    _coroutineFlags2[1] = true;
                }
            }

            // ※レイキャストで取得したかったけど、
            // 全然無理やったんで脳筋失礼します。
            if (mousePos.x >=  690.0f && mousePos.x <= 1230.0f &&
                mousePos.y >=  261.0f && mousePos.y <=  379.0f)
            {
                Debug.Log("タイトルへボタンに重なっています！");

                if (!_coroutineFlags[2])
                {
                    // カーソル移動音
                    AudioManager.instance.Play(SEPath.MoveCursor, AudioManager.ALL_VOLUME_VALUE);

                    StartCoroutine(
                    Scale(_images[2], _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[2]  = true;
                    _coroutineFlags2[2] = false;
                }
            }
            // タイトルへボタンが選ばれていなければ
            else
            {
                if (!_coroutineFlags2[2])
                {
                    StartCoroutine(
                    Scale(_images[2], _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlags[2]  = false;
                    _coroutineFlags2[2] = true;
                }
            }
        }

        // リトライボタンを押されたら
        if (_resultFlags[0])
        {
            // カウント開始
            _resultTimers[0] += Time.deltaTime;

            // "１秒後"
            if (_resultTimers[0] >= 1.0f)
            {
                // 遷移時の白画像の透明値を増やす
                if (_whiteColor.a <  1.0f) _whiteColor.a += Time.deltaTime;
                if (_whiteColor.a >= 1.0f) _whiteColor.a  = 1.0f;
            }

            // "2秒後"
            if (_resultTimers[0] >= 2.0f)
            {
                // ステージ番号保存
                var _stageNumber = PlayerPrefs.GetInt("stageIngex", 0);

                // ※敢えて設定しない事で同じシーンに遷移可能
                var NextSceneName = _stageNumber == 0 ? "Gimmick_Stage_Scene" :
                                    _stageNumber == 1 ? "Ice_Stage_Scene"     :
                                    _stageNumber == 2 ? "Fire_Stage_Scene"    :
                                    _stageNumber == 3 ? "Random"              :
                                    _stageNumber == 4 ? "Normal_Stage_Scene"  : "Water_Stage_Scene";

                WhiteLoading_Scene.SetNextScene(NextSceneName);
                SceneManager.LoadScene("WhiteLoading_Scene");
            }
        }

        // モード選択ボタンを押されたら
        if (_resultFlags[1])
        {
            // カウント開始
            _resultTimers[1] += Time.deltaTime;

            // "１秒後"
            if (_resultTimers[1] >= 1.0f)
            {
                // 遷移時の白画像の透明値を増やす
                if (_whiteColor.a < 1.0f) _whiteColor.a += Time.deltaTime;
                if (_whiteColor.a >= 1.0f) _whiteColor.a = 1.0f;
            }

            // "2秒後"
            if (_resultTimers[1] >= 2.0f)
            {  
                // ロードシーンを挟んでモード選択シーンに遷移
                WhiteLoading_Scene.SetNextScene("ModeSelect");
                SceneManager.LoadScene("WhiteLoading_Scene");
            }
        }

        // タイトルへボタンを押されたら
        if (_resultFlags[2])
        {
            // カウント開始
            _resultTimers[2] += Time.deltaTime;

            // "１秒後"
            if (_resultTimers[2] >= 1.0f)
            {
                // 遷移時の白画像の透明値を増やす
                if (_whiteColor.a < 1.0f) _whiteColor.a += Time.deltaTime;
                if (_whiteColor.a >= 1.0f) _whiteColor.a = 1.0f;
            }

            // "2秒後"
            if (_resultTimers[2] >= 2.0f)
            {
                // ロードシーンを挟んでタイトルシーンに遷移
                WhiteLoading_Scene.SetNextScene("Title_Scene");
                SceneManager.LoadScene("WhiteLoading_Scene");
            }
        }

        // 遷移時の白画像の色更新
        _whiteImage.color = _whiteColor;
    }

    // リトライボタンがクリックされたときの処理
    private void OnRetryButtonClick()
    {
        Debug.Log("リトライボタンがクリックされました！");

        AudioManager.instance.Play(SEPath.AllDecisions, AudioManager.ALL_VOLUME_VALUE);

        // リトライフラグ"ON"
        _resultFlags[0] = true;
    }

    // モード選択ボタンがクリックされたときの処理
    private void OnModeSelectButtonClick()
    {
        Debug.Log("モード選択ボタンがクリックされました！");

        AudioManager.instance.Play(SEPath.AllDecisions, AudioManager.ALL_VOLUME_VALUE);

        // モード選択フラグ"ON"
        _resultFlags[1] = true;
    }

    // タイトルへボタンがクリックされたときの処理
    private void OnTitleBackButtonClick()
    {
        Debug.Log("タイトルへボタンがクリックされました！");

        AudioManager.instance.Play(SEPath.AllDecisions, AudioManager.ALL_VOLUME_VALUE);

        // タイトルへフラグ"ON"
        _resultFlags[2] = true;
    }

    public IEnumerator Scale(Image transform, Vector3 destinationScale, float seconds, Easing.Ease easing)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定s
        Vector3 staScale = transform.rectTransform.sizeDelta;
        Vector3 endScale = destinationScale;
        // 初期地点と目標地点の差
        Vector3 difScale = endScale - staScale;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                transform.rectTransform.sizeDelta = endScale;
                break;
            }
            Vector3 nextPos = staScale + Ease(e) * difScale;
            transform.rectTransform.sizeDelta = nextPos;
        }
    }
}
