using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Idle_Character_Manager : MonoBehaviour
{
    // モデルスクリプト
    [SerializeField]
    public Idle_Character_Model _idle_Character_Model;

    // アニメーション登録用リスト
    private List<CharacterAnimation> animations = new List<CharacterAnimation>();

    // モデル生成フラグ
    [SerializeField]
    public bool _generateFlag = false;

    // 自身のモデルか判定フラグ
    [SerializeField]
    public bool _myModel = false;

    // Start is called before the first frame update
    void Start()
    {
        if (_myModel)
        {
            var _bodyNumber   = PlayerPrefs.GetInt("body",  0); 
            var _legNumber    = PlayerPrefs.GetInt("leg",   0);  
            
            _idle_Character_Model?.GenerateAndRegisterParts(this, _legNumber, _bodyNumber);
        }
    }

    public void ModelGenerate(int legNumber, int bodyNumber)
    {
        // CharacterModelにパーツ生成を指示
        _idle_Character_Model?.GenerateAndRegisterParts(this, legNumber, bodyNumber);

        // 生成フラグ"ON"
        _generateFlag = true;
    }

    public void ModelDestory()
    {
        // 子オブジェクトをクリア
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }

        // モデルスクリプト側でも削除を知らせる
        _idle_Character_Model.ModelDestory();

        // 生成フラグ"OFF"
        _generateFlag = false;

        // アニメーションリスト全削除
        animations.Clear();
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
        if (_generateFlag) SetAnimations(AnimationType.Walk);
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
