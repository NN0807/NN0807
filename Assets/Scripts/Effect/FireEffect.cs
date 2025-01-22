using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    // 入力処理
    [SerializeField]
    private HakopanControls _InputActions;

    // 剛体
    [SerializeField]
    private Rigidbody _rigidbody;

    public Vector3 n;
    public float p;

    void Start()
    {
        _InputActions = new HakopanControls();
        _InputActions.Enable();
        _rigidbody = GetComponent<Rigidbody>();

        n = new Vector3(0, 1, 0);
        p = 200.0f;

    }

    // Update is called once per frame
    void Update()
    {
        // ポーズ
        if (_InputActions.Player.Pause.triggered)
        {
            // 吹っ飛ばす
            _rigidbody.AddForce(n * p, ForceMode.Impulse);
        }
    }


}
