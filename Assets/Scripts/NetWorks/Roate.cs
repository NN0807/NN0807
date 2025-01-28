using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotate : MonoBehaviour
{

    public Image image;
    public float rotationSpeed = 100f; // 回転速度
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // Z軸を中心に回転させる
        image.rectTransform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
