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


    //スタートと終わりの目印
    public Vector3 startMarker = new Vector3(0.0f, 0.0f, 0.0f);
    public Vector3 endMarker = new Vector3(0.402f, -0.222f, 0.0f);


    public float duration = 1.0f;
    private float timeElapsed = 0.0f;
    private bool isReturning = false;

    // Start is called before the first frame update
    void Start()
    {
        // CharacterModelにパーツ生成を指示
        _title_Character_Model?.GenerateAndRegisterParts(this);
    }

    // パーツ登録
    public void RegisterPart(GameObject part)
    {
        // 各種キャラクタースクリプトを取得してリストに追加
        var animation = part.GetComponent<CharacterAnimation>();
        if (animation != null) animations.Add(animation);
    }

    // Update is called once per frame
    void Update()
    {
        // 常に待機アニメーションをさせる
        SetAnimations(AnimationType.Idle);

        // 現在の位置
        //float present_Location = (Time.time * speed) / distance_two;
        // オブジェクトの移動(ここだけ変わった！)
        //transform.position = Vector3.Slerp(startMarker, endMarker, present_Location);

        if (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / duration;
            t = Mathf.SmoothStep(0, 1, t); // スムーズな補間

            if (isReturning)
            {
                transform.position = Vector3.Lerp(endMarker, startMarker, t);
            }
            else
            {
                transform.position = Vector3.Lerp(startMarker, endMarker, t);
            }
        }
        else
        {
            if (!isReturning)
            {
                isReturning = true;
                timeElapsed = 0.0f; // 戻るためにリセット
            }
        }
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
