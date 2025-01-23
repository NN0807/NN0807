using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title_Manager : MonoBehaviour
{

    public Title_Character_Manager _title_Character_Manager01;
    public Title_Character_Manager _title_Character_Manager02;
    public Title_Character_Manager _title_Character_Manager03;
    public Title_Character_Manager _title_Character_Manager04;

    public float timer = 0.0f;


    private void Start()
    {
        StartCoroutine(CallChildStarts());
    }

    private IEnumerator CallChildStarts()
    {

        yield return new WaitForSeconds(0.5f); // 適宜調整可能

        // 
        //foreach (Transform child in transform)
        //{
        //    child.gameObject.SendMessage("CustomStart", SendMessageOptions.DontRequireReceiver);
        //    yield return new WaitForSeconds(0.5f); // 適宜調整可能
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
