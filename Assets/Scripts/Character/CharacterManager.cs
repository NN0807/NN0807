using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;

public class CharacterManager : MonoBehaviour
{
    // モデルスクリプト
    [SerializeField]
    public CharacterModel     _characterModel;
    // 操作スクリプト
    [SerializeField]
    public CharacterOperation _characterOperation;
    // 移動スクリプト
    private CharacterMove     _characterMove;
    // UIスクリプト
    [SerializeField]
    public CharacterUI        _characterUI;

    // 生成されたキャラクターパーツ登録用リスト
    private List<ICharacterPart> characterParts  = new List<ICharacterPart>();
    // コライダー登録用リスト
    private List<CharacterCollider>  colliders   = new List<CharacterCollider>(); 
    // アニメーション登録用リスト
    private List<CharacterAnimation> animations  = new List<CharacterAnimation>();

    // キャラクター番号(Playerは"0" Enemyは"1～3")
    // インスペクター側で設定
    // ※生成位置を決める為に使う
    [SerializeField]
    public int _characterNumber = 0;


    // Start is called before the first frame update
    void Start()
    {
        // キャラクター番号から初期生成位置を設定する
        GenerateTransform _mt = TransformInfo._generateTransforms[_characterNumber];
        this.transform.position = _mt.Position;
        this.transform.rotation = _mt.Rotation;

        // 操作スクリプトを設定
        TryGetComponent(out _characterOperation);

        // CharacterModelにパーツ生成を指示
        _characterModel?.GenerateAndRegisterParts(this, _characterNumber);

        // コライダーイベントを設定
        RegisterCollidersEvent();

        // ダッシュイベントを設定
        RegisterDashEvent();
    }

    // パーツ登録
    public void RegisterPart(GameObject part)
    {
        // 全キャラクタースクリプトを初期化
        var characterPartsArray = part.GetComponents<ICharacterPart>();
        foreach (var characterPart in characterPartsArray)
        {
            characterParts.Add(characterPart);
            characterPart.Initialize(this);
        }

        // 各種キャラクタースクリプトを取得してリストに追加
        var collider  = part.GetComponent<CharacterCollider>();
        if (collider  != null)    colliders.Add(collider);
        var animation = part.GetComponent<CharacterAnimation>();
        if (animation != null)  animations.Add(animation);
        var move      = part.GetComponent<CharacterMove>();
        if (move      != null) _characterMove = move;
        var ui        = part.GetComponent<CharacterUI>();
        if (ui        != null) _characterUI = ui;
    }

    // Update is called once per frame
    void Update()
    {
        // 更新
        foreach (var part in characterParts)
        {
            part.UpdatePart(this);
        }

        _characterModel?.ModelUpdate(this, _characterNumber);
        _characterOperation?.OperationUpdate(this);
    }

    // 各種パーツの現在のアニメーションステートを取得する
    public string GetCurrentAnimations()
    {
        // 武器以外のアニメーション名を取得
        // ※武器の攻撃アニメーション以外のアニメーションステートが
        // 全て「EmptyState」の為、アニメーション名が他のパーツと異なる
        string _legAnimation  = animations[0].GetCurrentAnimation();
        string _bodyAnimation = animations[1].GetCurrentAnimation();

        // 異なるアニメーション名があれば "None" を返す
        if (_legAnimation != _bodyAnimation) return "None";

        // 一致した場合、そのアニメーション名を返す
        return _legAnimation;
    }

    // 現在キャラクターが攻撃アニメーション中かどうかを判定する関数
    public bool IsCurrentlyAttacking()
    {
        foreach (var animation in animations)
        {
            // 一つでもtrueがあればtrueを返す
            if (animation.GetAttackAnimationFlag()) return true;
        }
        return false; // 全てfalseであればfalseを返す
    }

    // 各種スクリプトにコライダーを通知してイベントを登録
    private void RegisterCollidersEvent()
    {
        foreach (var collider in colliders)
        {
            _characterOperation?.RegisterColliderEvent(collider);
            _characterMove?.RegisterColliderEvent(collider);
        }
    }

    // ダッシュイベントを登録する関数
    private void RegisterDashEvent()
    {
        _characterUI?  .RegisterOperationEvent(_characterOperation);
        _characterMove?.RegisterOperationEvent(_characterOperation);

        foreach (var animation in animations)
        {
            animation.RegisterOperationEvent(_characterOperation);
        }
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

    public bool    GetAnimationEvent()  { return animations[2].GetAnimationFlag();                                            }
                                                                                                                              
    public float   GetHorizontalInput() { return _characterOperation ? _characterOperation.GetHorizontalInput() : 0.0f;       }
                                                                                                                              
    public float   GetVerticalInput()   { return _characterOperation ? _characterOperation.GetVerticalInput()   : 0.0f;       }

    public Vector3 GetMoveForward()     { return _characterMove      ? _characterMove.GetMoveForward()          :Vector3.zero;}

    public bool    GetCanDashFlag()     { return _characterUI        ? _characterUI.GetCanDashFlag()            : false;      }
}
