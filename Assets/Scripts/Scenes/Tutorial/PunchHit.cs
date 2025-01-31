using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchHit : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sandbag"))
        {
            Animator sandbagAnimator = other.GetComponent<Animator>();
            if(sandbagAnimator != null)
            {
                sandbagAnimator.SetTrigger("Hit");
            }
        }
    }
}
