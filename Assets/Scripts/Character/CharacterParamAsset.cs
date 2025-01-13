using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character,Data", menuName = "ScriptableObjects/CreateCharacterParamAsset")]
public class CharacterParamAsset : ScriptableObject
{
    //public List<CharacterParam> CharacterParamList = new List<CharacterParam>();

    // 名前(String変数は上に書いた方が良い)
    public string CharacterName = "Temp";

    // 移動速度
    [SerializeField]
    public float MoveSpeed;

    // 回転速度
    [SerializeField]
    public float RotateSpeed;

    // 攻撃力
    [SerializeField]
    public float Attack;

    // 加速力
    [SerializeField]
    public float Acceleration;

    // 最大ダッシュ速度
    [SerializeField]
    public float MaxDashSpeed;  

    // 最大スタミナ量
    [SerializeField]
    public float MaxStamina;

    // カメラ感度
    [SerializeField]
    public float CameraSpeed;
}

[System.Serializable]
public class CharacterParam
{
    
}