using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI判定に必要
using UnityEngine.UI;           // Buttonコンポーネントに必要
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    // 入力マネージャー
    [SerializeField]
    public InputManager inputManager;

    // 選択ボタンオブジェクト
    public GameObject buttonToSelect;
    public GameObject lastSelected;

    // EmissionHDR
    public Material _material; 

    // Start is called before the first frame update
    void Start()
    {
        // 最初にボタンを選択状態にする
        EventSystem.current.SetSelectedGameObject(buttonToSelect);

        // SpriteRendererコンポーネントを取得
        _image        = GetComponent<Image>();

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
                _material.SetColor("_Color", Color.white * 1.3f);

                // ブルーム拡縮値を増やす
                if (_bloomScatterChangeFlag)
                {
                    // Time.deltaTime / 2.0fの間隔で"0.6"まで
                    if (_bloom.scatter.value <= 0.6f) _bloom.scatter.value += Time.deltaTime;
                    // "0.6"まで到達したらフラグをfalseに
                    if (_bloom.scatter.value >= 0.6f) _bloomScatterChangeFlag = false;

                }
                // ブルーム拡縮値を減らす
                if (!_bloomScatterChangeFlag)
                {
                    // ↑と同様に減らす
                    if (_bloom.scatter.value >= 0.3f) _bloom.scatter.value -= Time.deltaTime;
                    if (_bloom.scatter.value <= 0.3f) _bloomScatterChangeFlag = true;

                }
            }
            else
            {
                // シェーダーに色を渡す
                _material.SetColor("_Color", Color.white * 1.0f);
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

                // シェーダーに色を渡す
                _material.SetColor("_Color", Color.white * 1.3f);

                // ブルーム拡縮値を増やす
                if (_bloomScatterChangeFlag)
                {
                    // Time.deltaTime / 2.0fの間隔で"0.6"まで
                    if (_bloom.scatter.value <= 0.6f) _bloom.scatter.value += Time.deltaTime;
                    // "0.6"まで到達したらフラグをfalseに
                    if (_bloom.scatter.value >= 0.6f) _bloomScatterChangeFlag = false;

                }
                // ブルーム拡縮値を減らす
                if (!_bloomScatterChangeFlag)
                {
                    // ↑と同様に減らす
                    if (_bloom.scatter.value >= 0.3f) _bloom.scatter.value -= Time.deltaTime;
                    if (_bloom.scatter.value <= 0.3f) _bloomScatterChangeFlag = true;

                }
            }
            // スタートボタンが選ばれていなければ
            else
            {
                // シェーダーに色を渡す
                _material.SetColor("_Color", Color.white * 1.0f);
            }
        }
    }

    // スタートボタンがクリックされたときの処理
    private void OnStartButtonClick()
    {
        Debug.Log("スタートボタンがクリックされました！");
    }
}
