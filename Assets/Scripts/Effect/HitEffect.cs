using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    // 通常パンチのヒットエフェクト
    [SerializeField]
    public GameObject _normalHitEffectPrefab;

    // クリティカルパンチのヒットエフェクト
    [SerializeField]
    public GameObject _criticalHitEffectPrefab;

    private void Start()
    {
        // EffectManagerにヒットエフェクトを登録
        EffectManager.Instance.RegisterEffect("NormalHitEffect",     _normalHitEffectPrefab);
        EffectManager.Instance.RegisterEffect("CriticalHitEffect", _criticalHitEffectPrefab);
    }
}
