using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Back_Title : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 開始ボタンが押されたらゲームスタート
        if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("Title_Scene");
        }
    }
}
