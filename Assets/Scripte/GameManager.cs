using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary> �Q�[���I���t���O </summary>
    [ReadOnly]
    public bool gameFinishFLg = false;

    /// <summary> �|�[�Y�t���O </summary>
    [ReadOnly]
    public bool pauseFLg = false;

    // �X�V����
    private void Update()
    {
        // �Q�[�����I�����Ă�����
        // �Q�[���I������
        if (gameFinishFLg) { GameFinish(); }

        // �|�[�Y����
        Pause();
    }

    /// <summary> �Q�[���I������ </summary>
    void GameFinish()
    {
        // �V�[���J��
        // ���݂̓Q�[���𗎂Ƃ�
        Application.Quit();
    }

    // �|�[�Y(��)
    void Pause()
    {
        // ����P�L�[�Ń|�[�Y������
        if (Input.GetKeyDown(KeyCode.P)) { pauseFLg = !pauseFLg; }

        // �^�C���X�P�[���ύX����
        if(pauseFLg && Time.timeScale != 0f) 
        { 
            Time.timeScale = 0f; 
            Debug.Log("�|�[�Y���܂���"); 
        }
        if(!pauseFLg && Time.timeScale == 0f) 
        { 
            Time.timeScale = 1f;
            Debug.Log("�|�[�Y����");
        }        
    }
}
