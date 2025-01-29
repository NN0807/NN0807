using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageCommon;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class CubeManager : MonoBehaviour
{
    public RedCubeManager      _redCubeManager;
    public BlueCubeManager    _blueCubeManager;
    public GreenCubeManager  _greenCubeManager;
    public YellowCubManager _yellowCubeManager;

    // 生成されたキャラクターパーツ登録用リスト
    private List<IGimmickPart> _characterParts = new List<IGimmickPart>();

    bool a = true;

    // Start is called before the first frame update
    void Start()
    {
           _redCubeManager.Initialize(this);
          _blueCubeManager.Initialize(this);
         _greenCubeManager.Initialize(this);
        _yellowCubeManager.Initialize(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(a)
        {
            CreateNavMesh();
            a = !a;
        }
    }

    private void CreateNavMesh()
    {        
        // ナビメッシュサーフェイス生成
        NavMeshSurface navMeshSurface = gameObject.AddComponent<NavMeshSurface>();
        navMeshSurface.layerMask = LayerMask.GetMask("Stage");
        navMeshSurface.BuildNavMesh();
    }
}
