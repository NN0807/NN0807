using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // Singletonパターン: EffectManagerのインスタンスを1つだけ保持し、どこからでもアクセス可能にする
    public static StageManager Instance { get; private set; }

    private void Awake()
    {
        // ステージ番号保存
        var _stageNumber = PlayerPrefs.GetInt("stageIngex", 0);

        // ステージに応じたBGM再生処理
        switch(_stageNumber)
        {
            case 0:     // 積み木
                StartCoroutine(AudioManager.instance.StartFuncPlay(BGMPath.GimmickStageBGM, AudioManager.ALL_VOLUME_VALUE, 0f, 1f, true));
                break;
            case 1:     // アイス
                StartCoroutine(AudioManager.instance.StartFuncPlay(BGMPath.IceStageBGM, AudioManager.ALL_VOLUME_VALUE, 0f, 1f, true));
                break;
            case 2:     // ファイア
                StartCoroutine(AudioManager.instance.StartFuncPlay(BGMPath.FireStageBGM, AudioManager.ALL_VOLUME_VALUE, 0f, 1f, true));
                break;
            case 3:     // ランダム
                // 何もしない
                break;
            case 4:     // 闘技場
                StartCoroutine(AudioManager.instance.StartFuncPlay(BGMPath.NormalStageBGM, AudioManager.ALL_VOLUME_VALUE, 0f, 1f, true));
                break;
            case 5:     // 水
                StartCoroutine(AudioManager.instance.StartFuncPlay(BGMPath.WaterStageBGM, AudioManager.ALL_VOLUME_VALUE, 0f, 1f, true));
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
