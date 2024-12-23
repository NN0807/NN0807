using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Input System関連のAPI

public class CharacterOperation : MonoBehaviour
{
    private HakopanControls inputActions;

    // Start is called before the first frame update
    void Start()
    {
        inputActions = new HakopanControls();
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(inputActions.Player.Fire.triggered)
        {
            Debug.Log("ファイヤー");
        }

        if (inputActions.Player.Pause.triggered)
        {
            Debug.Log("ポーズ");
        }
    }
}
