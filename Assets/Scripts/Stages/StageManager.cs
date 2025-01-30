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

        // ※敢えて設定しない事で同じシーンに遷移可能
        //var NextSceneName = _stageNumber == 0 ? "Gimmick_Stage_Scene" :
        //                    _stageNumber == 1 ? "Ice_Stage_Scene" :
        //                    _stageNumber == 2 ? "Fire_Stage_Scene" :
        //                    _stageNumber == 3 ? "Random" :
        //                    _stageNumber == 4 ? "Normal_Stage_Scene" : "Water_Stage_Scene";
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
