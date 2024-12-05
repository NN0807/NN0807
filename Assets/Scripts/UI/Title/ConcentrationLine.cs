using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConcentrationLine : MonoBehaviour
{
    [SerializeField] GameObject obj;

    [SerializeField] 
    private float AmountOfRotation = 0.5f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // ‰ñ“]
        MoveObj();
    }

    private void MoveObj()
    {
        obj.GetComponent<RectTransform>().Rotate(0, 0, -AmountOfRotation);
    }
}
