using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;

public class CharacterManager : MonoBehaviour
{
    // モデルスクリプト
    [SerializeField]
    public CharacterModel _characterModel;
    // 操作スクリプト
    [SerializeField]
    public CharacterOperation _characterOperation;
    // 移動スクリプト
    private CharacterMove _characterMove;

    // 生成されたキャラクターパーツ登録用リスト
    private List<ICharacterPart> characterParts = new List<ICharacterPart>();

    // コライダー登録用リスト
    private List<CharacterCollider> colliders = new List<CharacterCollider>();
    // アニメーション登録用リスト
    private List<CharacterAnimation> animations = new List<CharacterAnimation>();

    // キャラクター番号(Playerは"0" Enemyは"1～3")
    // インスペクター側で設定
    // ※生成位置を決める為に使う
    [SerializeField]
    public int _characterNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        // CharacterModelにパーツ生成を指示
        _characterModel.GenerateAndRegisterParts(this, _characterNumber);

        // CharacterOperationにパーツのコライダーを通知
        RegisterCollidersToOperation();
    }

    // パーツ登録
    public void RegisterPart(GameObject part)
    {
        // 全キャラクタースクリプトを初期化
        var characterPart = part.GetComponent<ICharacterPart>();
        if (characterPart != null)
        {
            characterParts.Add(characterPart);
            characterPart.Initialize(this);
        }

        // 各種キャラクタースクリプトを取得してリストに追加
        var collider  = part.GetComponent<CharacterCollider>();
        if (collider != null)   colliders.Add(collider);
        var animation = part.GetComponent<CharacterAnimation>();
        if (animation != null) animations.Add(animation);
    }

    // Update is called once per frame
    void Update()
    {
        // 更新
        foreach (var part in characterParts)
        {
            part.UpdatePart(this);
        }

        _characterModel.ModelUpdate(this);
        _characterOperation.OperationUpdate(this);
    }

    // CharacterOperationにコライダーを通知してイベントを登録
    private void RegisterCollidersToOperation()
    {
        foreach (var collider in colliders)
        {
            _characterOperation.RegisterColliderEvent(collider);
        }
    }

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

    public bool  GetAnimationEvent()  { return animations[2].GetAnimationFlag(); }

    public float GetHorizontalInput() { return _characterOperation.GetHorizontalInput(); }

    public float GetVerticalInput()   { return _characterOperation.GetVerticalInput();   }
}
