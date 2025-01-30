using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{

    // 初期化用変数
    public static class Initialize
    {
        public static readonly Vector3    Vector3    = new Vector3   (0.0f, 0.0f, 0.0f      );
        public static readonly Quaternion Quaternion = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
    }


    // 定数
    public static class CharacterConst
    {
        public const int CHARACTER_NUM   = 1;
        public const int CONST_MODEL_NUM = 8;
    }

    // アニメーションの種類
    public enum AnimationType
    {
        Idle,
        Walk,
        Attack,
        Hit,
        HitEarlyExit
    }

    // パーツタイプ
    public enum PartsType
    {
        Leg = 0, // 脚部
        Body,    // 体部
        Weapon   // 武器
    }

    // 初期生成行列
    public struct GenerateTransform
    {
        public Vector3    Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3    Scale    { get; set; }

        public GenerateTransform(Vector3 position, Quaternion rotation, Vector3 scale)
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
        }
    }

    // 行列情報を格納するクラス
    public static class TransformInfo
    {
        public static readonly GenerateTransform[] _generateTransforms = new GenerateTransform[]
        {
            new GenerateTransform(new Vector3( 0.0f, 4.5f,  2.2f), Quaternion.Euler(0.0f, -180.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f)),
            new GenerateTransform(new Vector3( 0.0f, 4.5f, -2.0f), Quaternion.Euler(0.0f,    0.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f)),
            new GenerateTransform(new Vector3(-2.2f, 4.5f,  0.0f), Quaternion.Euler(0.0f,   90.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f)),
            new GenerateTransform(new Vector3( 2.2f, 4.5f,  0.0f), Quaternion.Euler(0.0f,  -90.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f))
        };
    }
}

