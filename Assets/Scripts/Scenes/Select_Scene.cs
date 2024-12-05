using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Select_Scene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 開始ボタンが押されたらゲームスタート
        if (Input.GetKey(KeyCode.A))
        {
            SceneManager.LoadScene("Water_Stage_Scene");
        }

        // 開始ボタンが押されたらゲームスタート
        if (Input.GetKey(KeyCode.S))
        {
            SceneManager.LoadScene("Ice_Stage_Scene");
        }

        // 開始ボタンが押されたらゲームスタート
        if (Input.GetKey(KeyCode.D))
        {
            SceneManager.LoadScene("Fire_Stage_Scene");
        }
    }
}
