using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI判定に必要
using UnityEngine.UI;           // Buttonコンポーネントに必要
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class StartButton_UI : MonoBehaviour
{
    // アタッチするボタン
    public Button _startButton;

    // ポストエフェクト
    [SerializeField]
    public Volume _volume;

    // ブルーム
    private Bloom _bloom;

    // ブルーム拡縮値変更フラグ
    private bool _bloomScatterChangeFlag = false;

    // キーマウ用のボタン画像
    public Sprite _startButtonSprite;

    // コントローラー用のボタン画像
    public Sprite _startAButtonSprite;

    // 画像
    private Image _image;

    // 白画像
    public Image _whiteImage;

    // 入力マネージャー
    [SerializeField]
    public InputManager inputManager;

    // 選択ボタンオブジェクト
    public GameObject buttonToSelect;

    // EmissionHDR
    public Material _material;

    // START画像の拡縮値と色
    private Vector3 _startScale  = new Vector3(540.0f, 118.0f, 1.0f);
    private Vector3 _targetScale = new Vector3(720.0f, 153.0f, 1.0f);
    // イージング時間
    [SerializeField]
    private float _easingTime = 0.3f;

    // コルーチンフラグ
    private bool _coroutineFlag  = false;
    private bool _coroutineFlag2 = false;

    private Color _whiteColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);

    private float _gameStartTimer = 0.0f;
    private bool _gameStart = false;

    // Start is called before the first frame update
    void Start()
    {
        // 最初にボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(buttonToSelect);

        // SpriteRendererコンポーネントを取得
        _image        = GetComponent<Image>();

        // 初期設定
        _image.rectTransform.sizeDelta = _startScale;
        _whiteImage.color = _whiteColor;
        _gameStartTimer = 0.0f;
        _gameStart = false;

        // ボタンにクリックイベントを登録
        if (_startButton != null)
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
        }

        // ポストエフェクト設定
        _volume.profile.TryGet(out _bloom);
        if (_bloom == null) 
        {
            Debug.Log("ブルームがありません！！！");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        // 現在の入力デバイスがゲームパッドであれば
        if (inputManager.GetCurrentInputDevice() == "Gamepad") 
        {
            // ゲームパッド用の画像に差替える
            _image.sprite = _startAButtonSprite;

            // 現在選択されているGameObjectを取得
            buttonToSelect = EventSystem.current.currentSelectedGameObject;

            // スタートボタンが有り、名前が"Start Button"であれば
            if (buttonToSelect != null && buttonToSelect.name == "Start Button")
            {
                // シェーダーに色を渡す
                //_material.SetColor("_Color", Color.white * 1.0f);

                if (!_coroutineFlag)
                {
                    StartCoroutine(
                    Scale(_image, _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlag  = true;
                    _coroutineFlag2 = false;
                }

                // ブルーム拡縮値を増やす
                if (_bloomScatterChangeFlag)
                {
                    // Time.deltaTime / 2.0fの間隔で"0.6"まで
                    //if (_bloom.scatter.value <= 0.6f) _bloom.scatter.value += Time.deltaTime;
                    // "0.6"まで到達したらフラグをfalseに
                    //if (_bloom.scatter.value >= 0.6f) _bloomScatterChangeFlag = false;

                }
                // ブルーム拡縮値を減らす
                if (!_bloomScatterChangeFlag)
                {
                    // ↑と同様に減らす
                    //if (_bloom.scatter.value >= 0.3f) _bloom.scatter.value -= Time.deltaTime;
                    //if (_bloom.scatter.value <= 0.3f) _bloomScatterChangeFlag = true;

                }
            }
            else
            {
                // シェーダーに色を渡す
                _material.SetColor("_Color", Color.white * 1.0f);

                if (!_coroutineFlag2)
                {
                    StartCoroutine(
                    Scale(_image, _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlag  = false;
                    _coroutineFlag2 = true;
                }
            }
        }
        // 現在の入力デバイスがキーマウであれば
        else if (inputManager.GetCurrentInputDevice() == "Keyboard/Mouse")
        {
            // ゲームパッド用の画像に差替える
            _image.sprite = _startButtonSprite;

            // マウスの位置を取得する
            Vector3 mousePos = Input.mousePosition;

            // ※レイキャストで取得したかったけど、
            // 全然無理やったんで脳筋失礼します。
            if (mousePos.x >= 690.0f && mousePos.x <= 1230.0f &&
                mousePos.y >= 197.0f && mousePos.y <= 315.0f)
            {
                Debug.Log("スタートボタンがに重なっています！");

                if (!_coroutineFlag)
                {
                    // カーソル移動音
                    AudioManager.instance.Play(SEPath.MoveCursor, 0.004f);

                    StartCoroutine(
                    Scale(_image, _targetScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlag  = true;
                    _coroutineFlag2 = false;
                }

                // シェーダーに色を渡す
                //_material.SetColor("_Color", Color.white * 1.3f);

                // ブルーム拡縮値を増やす
                if (_bloomScatterChangeFlag)
                {
                    // Time.deltaTime / 2.0fの間隔で"0.6"まで
                    //if (_bloom.scatter.value <= 0.6f) _bloom.scatter.value += Time.deltaTime;
                    // "0.6"まで到達したらフラグをfalseに
                    //if (_bloom.scatter.value >= 0.6f) _bloomScatterChangeFlag = false;

                }
                // ブルーム拡縮値を減らす
                if (!_bloomScatterChangeFlag)
                {
                    // ↑と同様に減らす
                    //if (_bloom.scatter.value >= 0.3f) _bloom.scatter.value -= Time.deltaTime;
                    //if (_bloom.scatter.value <= 0.3f) _bloomScatterChangeFlag = true;

                }
            }
            // スタートボタンが選ばれていなければ
            else
            {
                // シェーダーに色を渡す
                _material.SetColor("_Color", Color.white * 1.0f);

                if (!_coroutineFlag2)
                {
                    StartCoroutine(
                    Scale(_image, _startScale, _easingTime, Easing.Ease.OutSine));

                    _coroutineFlag  = false;
                    _coroutineFlag2 = true;
                }
            }
        }

        if (_gameStart)
        {
            _gameStartTimer += Time.deltaTime;

            if (_gameStartTimer >= 1.0f)
            {
                if (_whiteColor.a < 1.0f) _whiteColor.a += Time.deltaTime;
            }

            if (_gameStartTimer >= 2.0f)
            {
                WhiteLoading_Scene.SetNextScene("ModeSelect");
                SceneManager.LoadScene("WhiteLoading_Scene");
            }
        }

        _whiteImage.color = _whiteColor;
    }

    // スタートボタンがクリックされたときの処理
    private void OnStartButtonClick()
    {
        Debug.Log("スタートボタンがクリックされました！");


        AudioManager.instance.Play(SEPath.AllDecisions, 0.004f);

        _gameStart = true;
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
