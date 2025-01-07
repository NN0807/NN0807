using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterCollider : MonoBehaviour, ICharacterPart
{
    // 衝突中(Effect)イベント
    public event Action CollisionEFKStayEvent;
    // 衝突終(Effect)イベント
    public event Action CollisionEFKExitEvent;

    public void Initialize(CharacterManager manager)
    {
        Debug.Log("CharacterCollider 初期化");
    }

    public void UpdatePart(CharacterManager manager)
    {
        Debug.Log("CharacterCollider　更新処理");
    }


    // 当たっている間に呼ばれる関数
    void OnCollisionStay(Collision collision)
    {
        // Effectと衝突したら
        if (collision.gameObject.CompareTag("Effect"))
        {
            Debug.Log("Effectタグのオブジェクトに衝突しました");

            // イベント発火
            CollisionEFKStayEvent?.Invoke();
        }
    }

    // 離れたら呼ばれる関数
    void OnCollisionExit(Collision collision)
    {
        // Effectとの衝突から離れたら
        if (collision.gameObject.CompareTag("Effect"))
        {
            Debug.Log("Effectタグのオブジェクトから離れました");

            // イベント発火
            CollisionEFKExitEvent?.Invoke();
        }
    }
}
