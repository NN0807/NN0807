using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;

public class CharacterManager : MonoBehaviour
{
    // モデルスクリプト
    [SerializeField]
<<<<<<< HEAD
    public CharacterModel     _characterModel;
=======
    public CharacterModel _characterModel;
>>>>>>> 13eac9b5a4e511597ad99283e019505976c7e0ab
    // 操作スクリプト
    [SerializeField]
    public CharacterOperation _characterOperation;
    // 移動スクリプト
<<<<<<< HEAD
    private CharacterMove     _characterMove;
=======
    private CharacterMove _characterMove;
>>>>>>> 13eac9b5a4e511597ad99283e019505976c7e0ab

    // 生成されたキャラクターパーツ登録用リスト
    private List<ICharacterPart> characterParts = new List<ICharacterPart>();

    // コライダー登録用リスト
<<<<<<< HEAD
    private List<CharacterCollider>  colliders   = new List<CharacterCollider>(); 
=======
    private List<CharacterCollider> colliders = new List<CharacterCollider>();
>>>>>>> 13eac9b5a4e511597ad99283e019505976c7e0ab
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
        // CharacterModelにパーツ生成を指示
<<<<<<< HEAD
        _characterModel?.GenerateAndRegisterParts(this, _characterNumber);
=======
        _characterModel.GenerateAndRegisterParts(this, _characterNumber);
>>>>>>> 13eac9b5a4e511597ad99283e019505976c7e0ab

        // コライダーイベントを設定
        RegisterCollidersEvent();
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
        if (collider  != null)    colliders.Add(collider);
        var animation = part.GetComponent<CharacterAnimation>();
        if (animation != null)  animations.Add(animation);
        var move      = part.GetComponent<CharacterMove>();
        if (move      != null) _characterMove = move;
    }

    // Update is called once per frame
    void Update()
    {
        // 更新
        foreach (var part in characterParts)
        {
            part.UpdatePart(this);
        }

        _characterModel?.ModelUpdate(this);
        _characterOperation?.OperationUpdate(this);
    }

    // 各種パーツの現在のアニメーションステートを取得する
    public string GetCurrentAnimations()
    {
        // 0番(脚部)のアニメーション名を取得
        string _firstAnimation = animations[0].GetCurrentAnimation();

        foreach (var animator in animations)
        {
            // 各Animatorのアニメーション名を取得
            string _currentAnimation = animator.GetCurrentAnimation();

            // 一度でも異なるアニメーション名があれば "None" を返す
            if (_currentAnimation != _firstAnimation)
            {
                return "None";
            }
        }

        // 全員一致した場合、そのアニメーション名を返す
        return _firstAnimation;
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

    public bool    GetAnimationEvent()  { return animations[2].GetAnimationFlag();         }

    public float   GetHorizontalInput() { return _characterOperation.GetHorizontalInput(); }

    public float   GetVerticalInput()   { return _characterOperation.GetVerticalInput();   }

    public Vector3 GetMoveForward()     { return _characterMove.GetMoveForward();          }
}
