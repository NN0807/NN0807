using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fight_UI : MonoBehaviour
{
    [SerializeField]
    public Image _fightImage;

    // 演出開始フラグ
    public bool _playEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        _fightImage = GetComponent<Image>();

        _fightImage.rectTransform.localScale = new Vector3(3.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(_playEffect)
        {

        }
    }
}
