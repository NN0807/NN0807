using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectManager : MonoBehaviour
{

    public void LoadBattleMode()
    {
        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, 0.004f);

        SceneManager.LoadScene("CustomizeScene");
    }

    public void LoadOnlineMode()
    {
        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, 0.004f);

        // SceneManager.LoadScene("OnlineMode");
    }

    public void LoadTutorialMode()
    {
        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, 0.004f);

        // SceneManager.LoadScene("TutorialScene");
    }
}
