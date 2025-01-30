using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideVenueCollider : MonoBehaviour
{
    // キャラが衝突したら
    void OnCollisionEnter(Collision collision)
    {
        // Enemy(Player)と衝突したら
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player")))
        {
            collision.transform.root.gameObject.GetComponent<CharacterManager>().Dead();

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
