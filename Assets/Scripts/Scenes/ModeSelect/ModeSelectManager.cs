using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectManager : MonoBehaviour
{

    public void LoadBattleMode()
    {
        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, AudioManager.ALL_VOLUME_VALUE);

        SceneManager.LoadScene("CustomizeScene");
    }

    public void LoadOnlineMode()
    {
        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, AudioManager.ALL_VOLUME_VALUE);

        // SceneManager.LoadScene("OnlineMode");
    }

    public void LoadTutorialMode()
    {
        // 決定音
        AudioManager.instance.Play(SEPath.AllDecisions, AudioManager.ALL_VOLUME_VALUE);

        // SceneManager.LoadScene("TutorialScene");
    }
}
