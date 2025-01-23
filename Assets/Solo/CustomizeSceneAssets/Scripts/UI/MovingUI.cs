using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingUI : MonoBehaviour
{
    /// <summary>
    /// 動く方向
    /// </summary>
    [SerializeField]
    private Vector3 moveVec = default;

    /// <summary>
    /// 動くスピード
    /// </summary>
    [SerializeField]
    private float speed = default;

    /// <summary>
    /// 動く距離
    /// </summary>
    [SerializeField]
    private float length = default;

    /// <summary>
    /// 初期位置
    /// </summary>
    private Vector3 initialPosition;

    /// <summary>
    /// 動いた距離
    /// </summary>
    private float movedDistance = 0f;

    private void Start()
    {
        // ローカル初期位置を保存
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // 親の動きに対応するためローカル座標を基準に動作
        float step = speed * Time.deltaTime;
        movedDistance += step;

        // 指定距離を超えていない場合は移動
        if (movedDistance <= length)
        {
            transform.localPosition += moveVec.normalized * step;
        }
        else
        {
            // 初期ローカル位置に戻る
            transform.localPosition = initialPosition;
            movedDistance = 0f;
        }
    }
}
