using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // UI判定に必要
using UnityEngine.UI;           // Buttonコンポーネントに必要

public class StartButton_UI : MonoBehaviour
{
    // アタッチするボタン
    public Button _startButton;

    // Start is called before the first frame update
    void Start()
    {
        // ボタンにクリックイベントを登録
        if (_startButton != null)
        {
            _startButton.onClick.AddListener(OnStartButtonClick);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        //if (mousePos.x >= 690.0f && mousePos.x <= 1230.0f&&
        //    mousePos.y<=225.0f-) 
        {
            Debug.Log("スタートボタンがに重なっています！");
        }
    }

    // スタートボタンがクリックされたときの処理
    private void OnStartButtonClick()
    {
        Debug.Log("スタートボタンがクリックされました！");
    }
}
