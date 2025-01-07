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
}