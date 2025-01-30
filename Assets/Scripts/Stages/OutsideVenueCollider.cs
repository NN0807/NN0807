using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideVenueCollider : MonoBehaviour
{

    public int[] _number = new int[4];

    // キャラが衝突したら
    void OnCollisionEnter(Collision collision)
    {
        // Enemy(Player)と衝突したら
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")))
        {
            if (_number[collision.transform.root.gameObject.GetComponent<CharacterManager>()._characterNumber] == 1)
                return;

            collision.transform.root.gameObject.GetComponent<CharacterManager>().Dead();

            _number[collision.transform.root.gameObject.GetComponent<CharacterManager>()._characterNumber] = 1;


            // プレイヤーが死亡したら
            if (collision.transform.root.gameObject.GetComponent<CharacterManager>()._characterNumber == 0)
            {
                // 自身のゲーム順位を決定
                GameManager.Instance.SetMyRanking(GameManager.Instance.GetDeadCharacterCount());

                Debug.Log("順位決定");
            }

            // 死亡カウント
            GameManager.Instance.AddDeadCharacterCount();

            Debug.Log("死亡");
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        for (int Index = 0; Index < 4; Index++)
        {
            _number[Index] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
