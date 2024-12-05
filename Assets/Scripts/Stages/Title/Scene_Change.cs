using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Change : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 開始ボタンが押されたらゲームスタート
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Select_Scene");
        }
    }
}
