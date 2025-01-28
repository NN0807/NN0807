using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Log : MonoBehaviour
{
    // 開始地点と中間地点と終了地点の目印
    public Vector3 _startPos  = new Vector3(0.0f, 0.035f, -6.00f);
    public Vector3 _endPos    = new Vector3(0.0f, 0.035f, -0.95f);

    // イージング時間
    [SerializeField]
    public float _easingTime = 1.5f;

    public void CustomStart()
    {
        Debug.Log($"{gameObject.name} のCustomStartが呼ばれました");

        // 開始地点を設定
        this.transform.position = _startPos;

        // 引数のEnumを変えるだけで、イージング関数の差し替えができる
        // EaseOutQuadで、絶対座標で_endPosの位置に1秒かけて移動させる
        StartCoroutine(
            Move(this.transform, _endPos, _easingTime, Easing.Ease.OutBack, true)
        );
    }

    // Start is called before the first frame update
    void Start()
    {
        //// 開始地点を設定
        //this.transform.position = _startPos;

        //// 引数のEnumを変えるだけで、イージング関数の差し替えができる
        //// EaseOutQuadで、絶対座標で_endPosの位置に1秒かけて移動させる
        //StartCoroutine(
        //    Move(this.transform, _endPos, _easingTime, Easing.Ease.OutBack, true)
        //);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 指定したTransformの座標を更新する
    // 引数 Transform, 目的値の座標、何秒で動かすか、イージングの種類、移動先が絶対座標かどうか
    public IEnumerator Move(Transform transform, Vector3 destinationPos, float seconds, Easing.Ease easing, bool absolute)
    {
        // イージング関数の取得
        var Ease = Easing.GetEasingMethod(easing);

        // 現在点と移動先の設定
        Vector3 staPos = transform.localPosition;
        Vector3 endPos = absolute ? destinationPos : staPos + destinationPos;
        // 初期地点と目標地点の差
        Vector3 difPos = endPos - staPos;

        // N秒かけて移動させる
        float e = 0;
        while (true)
        {
            yield return null;
            e += Time.deltaTime / seconds;
            if (e >= 1.0f)
            {
                transform.localPosition = endPos;
                break;
            }
            Vector3 nextPos = staPos + Ease(e) * difPos;
            transform.localPosition = nextPos;
        }
    }
}
