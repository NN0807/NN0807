using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    // Singletonパターン: EffectManagerのインスタンスを1つだけ保持し、どこからでもアクセス可能にする
    public static EffectManager Instance { get; private set; }

    // ※Dictionaryクラスはインデックス番号の代わりにKey（キー）と呼ばれる名前を使い、
    // セットでValue（バリュー）と呼ばれる値を扱います。
    // 配列やListでindex（インデックス）番号で検索する代わりに、
    // Key（例えば文字）で検索ができる。
    // エフェクトの名前をKeyに、プレハブを管理する配列
    private Dictionary<string, GameObject> _effectPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        // Singletonインスタンスがまだ存在しない場合、現在のインスタンスを設定
        if (Instance == null)
        {
            // インスタンスを設定
            Instance = this;  
        }
        else
        {
            // すでにインスタンスが存在する場合、現在のオブジェクトを削除
            Destroy(gameObject); 
        }
    }

    // RegisterEffect: 新しいエフェクトをEffectManagerに登録する関数
    public void RegisterEffect(string effectName, GameObject effectPrefab)
    {
        // 登録されていないエフェクト名のみ登録する
        if (!_effectPrefabs.ContainsKey(effectName))
        {
            // 名前とプレハブを配列に追加
            _effectPrefabs.Add(effectName, effectPrefab);  
        }
    }

    // PlayEffect: 名前に対応するエフェクトを指定位置で再生する関数
    public void PlayEffect(string effectName, Vector3 position)
    {
        // 配列からエフェクト名に対応するプレハブを取得
        if (_effectPrefabs.TryGetValue(effectName, out GameObject effectPrefab))
        {
            // プレハブが存在した場合、指定位置にインスタンスを生成
            Instantiate(effectPrefab, position, Quaternion.identity);
        }
        else
        {
            // プレハブが見つからなかった場合、警告を表示
            Debug.LogWarning($"エフェクト名： {effectName} がありません！！");
        }
    }
}