using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectManager : MonoBehaviour
{

    public void LoadBattleMode()
    {
        SceneManager.LoadScene("CustomizeScene");
    }

    public void LoadOnlineMode()
    {
        // SceneManager.LoadScene("OnlineMode");
    }

    public void LoadTutorialMode()
    {
        // SceneManager.LoadScene("TutorialScene");
    }
}
