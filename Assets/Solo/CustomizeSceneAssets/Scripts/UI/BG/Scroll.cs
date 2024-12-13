using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    ///<summary>生成するスクロール画像プレハブ</summary>
    [SerializeField]
    private GameObject ScrollPrefab = null;

    ///<summary>生成している画像オブジェクト</summary>
    [SerializeField]
    private GameObject[] ScrollObject = new GameObject[2];

    private void Awake()
    {
        // スクロール画像生成
        ScrollObject[0] = Instantiate(ScrollPrefab, transform).gameObject;

        // 次の画像も生成
        ScrollObject[1] = Instantiate(ScrollPrefab, transform).gameObject;
        ScrollObject[1].transform.localPosition = new Vector3(1920f, 0f, 0f);
    }

    private void Update()
    {
        // スクロール処理
        MoveScroll();

        // 画面から外側に出た画像を開始位置に戻す
        ResetPos();
    }

    ///<summary>スクロール</summary>
    private void MoveScroll()
    {
        // 画像をスクロール
        for (int i = 0; i < 2; i++)
        {
            ScrollObject[i].transform.localPosition -= new Vector3(2f, 0f, 0f);
        }
    }

    ///<summary>スクロールしきった画像を開始位置に戻す</summary>
    private void ResetPos()
    {
        // 各画像が画面範囲を超えたら次の画像スクロール開始位置に戻す
        for (int i = 0; i < 2; i++)
        {
            if (ScrollObject[i].transform.localPosition.x < -1920f)
            {
                ScrollObject[i].transform.localPosition = new Vector3(1920f, 0f, 0f);
            }
        }
    }
}
