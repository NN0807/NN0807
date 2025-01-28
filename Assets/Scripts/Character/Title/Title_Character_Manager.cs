using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Title_Character_Manager : MonoBehaviour
{
    // モデルスクリプト
    [SerializeField]
    public Title_Character_Model _title_Character_Model;

    // アニメーション登録用リスト
    private List<CharacterAnimation> animations = new List<CharacterAnimation>();

    [SerializeField]
    public int Number = 0;

    public void CustomStart()
    {
        Debug.Log($"{gameObject.name} のCustomStartが呼ばれました");

        // CharacterModelにパーツ生成を指示
        _title_Character_Model?.GenerateAndRegisterParts(this, Number);
    }

    // Start is called before the first frame update
    void Start()
    {
        // CharacterModelにパーツ生成を指示
        //_title_Character_Model?.GenerateAndRegisterParts(this, Number);
    }

    // パーツ登録
    public void RegisterPart(GameObject part)
    {
        // 各種キャラクタースクリプトを取得してリストに追加
        var animationArray = part.GetComponentsInChildren<CharacterAnimation>();
        foreach (var animation in animationArray)
        {
            animations.Add(animation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 常に待機アニメーションをさせる
        SetAnimations(AnimationType.Idle);
    }

    // アニメーション起動
    public void SetAnimations(AnimationType animationType)
    {
        foreach (var animation in animations)
        {
            if      (animationType == AnimationType.Idle)   animation.SetIdleAnimation();
            else if (animationType == AnimationType.Walk)   animation.SetWalkAnimation();
            else if (animationType == AnimationType.Attack) animation.SetAttackAnimation();
            else if (animationType == AnimationType.Hit)    animation.SetHitAnimation();
        }
    }
}
