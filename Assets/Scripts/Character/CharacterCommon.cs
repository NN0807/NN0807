using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    // 定数
    public static class CharacterConst
    {
        public const int CONST_MODEL_NUM = 8;
    }

    // アニメーションの種類
    public enum AnimationType
    {
        Walk,
        Idle,
        Attack,
        Hit
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
            new GenerateTransform(new Vector3( 0.0f, 2.5f,  2.8f), Quaternion.Euler(0.0f,   0.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f)),
            new GenerateTransform(new Vector3( 0.0f, 2.5f, -2.8f), Quaternion.Euler(0.0f, 180.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f)),
            new GenerateTransform(new Vector3(-2.8f, 2.5f,  0.0f), Quaternion.Euler(0.0f, -90.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f)),
            new GenerateTransform(new Vector3( 2.8f, 2.5f,  0.0f), Quaternion.Euler(0.0f,  90.0f, 0.0f), new Vector3(1.5f, 1.5f, 1.5f))
        };
    }
}

