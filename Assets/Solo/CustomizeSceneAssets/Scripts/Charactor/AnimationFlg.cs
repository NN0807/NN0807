using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFlg : MonoBehaviour
{
    public bool animationFlg;

    public bool startSlowAnimationFlg;
    public bool endSlowAnimationFlg;

    public void EnbleFlg()
    {
        animationFlg = true;
    }

    public void DisableFlg()
    {
        animationFlg = false;
    }

    public void StartSlowAnimationFlg()
    {
        startSlowAnimationFlg = true;
    }
    public void EndSlowAnimationFlg()
    {
        endSlowAnimationFlg = true;
    }
}
