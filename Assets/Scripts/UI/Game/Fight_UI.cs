using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fight_UI : MonoBehaviour
{
    // FIGHT画像
    [SerializeField]
    public Image _fightImage;

    // 演出開始フラグ
    public bool _playEffect = false;

    // FIGTH画像の拡縮値と色
    private Vector3  _startScale  = new Vector3(3640.0f,    0.0f, 1.0f      );
    private Vector3 _targetScale  = new Vector3(1920.0f, 1080.0f, 1.0f      );
    private Color    _startColor  = new Color  (   1.0f,    1.0f, 1.0f, 0.0f);
    private Color   _targetColor  = new Color  (   1.0f,    1.0f, 1.0f, 1.0f);

    private Vector3 _startScale2  = new Vector3(1920.0f, 1080.0f, 1.0f      );
    private Vector3 _targetScale2 = new Vector3(3640.0f, 2160.0f, 1.0f      );
    private Color   _startColor2  = new Color  (   1.0f,    1.0f, 1.0f, 1.0f);
    private Color  _targetColor2  = new Color  (   1.0f,    1.0f, 1.0f, 0.0f);

    // イージング時間
    [SerializeField]
    private float _easingTime = 1.0f;

    // 演出"2"の開始タイミングを計るタイマー
    private float _playEffect2Timer = 0.0f;

    // コルーチンフラグ
    private bool _coroutineFlag  = false;
    private bool _coroutineFlag2 = false;

    // Start is called before the first frame update
    void Start()
    {
        // 画像取得
        _fightImage = GetComponent<Image>();

        // 初期設定
        _fightImage.rectTransform.sizeDelta = _startScale;
        _fightImage.color                   = _startColor;
    }

    // Update is called once per frame
    void Update()
    {
        // 演出開始
        if(_playEffect)
        {
            if(!_coroutineFlag)
            {
                StartCoroutine(
                Scale(_fightImage, _targetScale, _easingTime, Easing.Ease.OutSine));

                StartCoroutine(
                Alpha(_fightImage, _targetColor, _easingTime, Easing.Ease.OutSine));

                _coroutineFlag = true;
            }

            _playEffect2Timer += Time.deltaTime;

            // 演出"2"開始
            if (_playEffect2Timer > 1.2f && !_coroutineFlag2) 
            {
                StartCoroutine(
                Scale(_fightImage, _targetScale2, _easingTime, Easing.Ease.OutSine));

                StartCoroutine(
                Alpha(_fightImage, _targetColor2, _easingTime, Easing.Ease.OutSine));

                _coroutineFlag2 = true;
            }
        }
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

    //UnityEditor.TransformWorldPlacementJSON:{"position":{"x":0.0,"y":7.940000057220459,"z":5.119999885559082},
    //"rotation":{"x":0.0,"y":-0.8870108723640442,"z":0.4617486298084259,"w":0.0},
    //"scale":{"x":1.0,"y":1.0,"z":1.0}}

    public IEnumerator Alpha(Image image, Color destinationAlpha, float seconds, Easing.Ease easing)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定s
        Color staAlpha = image.color;
        Color endAlpha = destinationAlpha;
        // 初期地点と目標地点の差
        Color difAlpha = endAlpha - staAlpha;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                image.color = endAlpha;
                break;
            }
            Color nextPos = staAlpha + Ease(e) * difAlpha;
            image.color = nextPos;
        }
    }
}
