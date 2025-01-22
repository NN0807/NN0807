using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageCommon;

public class RedCubeManager : MonoBehaviour, IGimmickPart
{
    // 赤色Cube
    [SerializeField]
    public GameObject RedCube;

    private int _roopCount = 0;

    // 初期化関数
    public void Initialize(CubeManager manager)
    {
        for (int horizontal = 0; horizontal < 7; horizontal++)
        {
            for (int vertical = 0; vertical < 7; vertical++)
            {
                // 生成座標算出
                Vector3 Position = CalculatePosition(horizontal, vertical, StageConst.START_X, StageConst.START_Z, StageConst.CUBE_SIZE);

                // Cubeを配置
                if (_roopCount % 4 == 2) Instantiate(RedCube, Position, Quaternion.identity, this.transform);

                _roopCount++;
            }
        }
    }

    // 更新関数
    public void UpdatePart(CubeManager manager)
    {

    }

    void Awake()
    {
        _roopCount = 0;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 生成座標計算関数
    Vector3 CalculatePosition(int row, int col, float startX, float startZ, float size)
    {
        float x = startX + col * size;
        float z = startZ - row * size;
        return new Vector3(x, 0, z);
    }
}

