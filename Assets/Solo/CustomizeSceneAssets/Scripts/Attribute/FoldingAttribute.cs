using UnityEngine;

// 折り畳み開始
public class StartFoldingAttribute : PropertyAttribute
{
    public string foldName;

    // コンストラクタ
    public StartFoldingAttribute(string name)
    {
        this.foldName = name;
    }
}

// 折り畳み終了
public class EndFoldingAttribute : PropertyAttribute
{

}