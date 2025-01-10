using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAttribute : PropertyAttribute
{
    /// <summary>
    /// 実行する関数名
    /// </summary>
    public string methodName;

    /// <summary>
    /// ボタンに表示する関数名
    /// </summary>
    public string buttonName;

    public ButtonAttribute(string methodName, string buttonName = null)
    {
        this.methodName = methodName;

        if (buttonName == null)
        {
            this.buttonName = methodName;
        }
        else
        {
            this.buttonName = buttonName;
        }

    }
}
