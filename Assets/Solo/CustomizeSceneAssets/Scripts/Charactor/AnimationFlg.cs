using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFlg : MonoBehaviour
{
    public bool animationFlg;

    public void EnbleFlg()
    {
        animationFlg = true;
    }

    public void DisableFlg()
    {
        animationFlg = false;
    }
}
