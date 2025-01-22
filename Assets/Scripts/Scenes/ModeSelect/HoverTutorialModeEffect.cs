using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverTutorialModeEffect : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{

    // 桜プレハブの配列
    public GameObject[] sakuraPrefabs;
    // 桜を配置する親オブジェクト
    public RectTransform sakuraParent;

    // マスク用のRectTransform
    public RectTransform maskArea;

    // 桜を生成する間隔
    public float spawnInterval = 1f;

    // 最小生成枚数
    public int minSakuraCount = 1;
    // 最大生成枚数
    public int maxSakuraCount = 3;

    // 桜の落下速度
    public float fallSpeed = 2f;

    // X方向の移動範囲
    public Vector2 fallRangeX = new Vector2(-100, -50);
    // Y方向の移動範囲
    public Vector2 fallRangeY = new Vector2(-75, -100);

    // X方向の生成範囲
    public Vector2 spawnOffsetX = new Vector2(0, 250);
    // Y方向の生成範囲
    public Vector2 spawnOffsetY = new Vector2(50, 200);

    // マウスが乗っているか
    public bool isHovered = false;

    // 現在生成されている桜のリスト
    private List<GameObject> activeSakuraList = new List<GameObject>();

    private void Start()
    {
        // マスク範囲をチュートリアルボタンのサイズに合わせる
        if(maskArea != null && sakuraParent != null)
        {
            sakuraParent.sizeDelta = maskArea.sizeDelta;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        StartCoroutine(SpawnSakura());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        ClearAllSakura();
    }

    private IEnumerator SpawnSakura()
    {
        while(isHovered)
        {
            int sakuraCount = Random.Range(minSakuraCount, maxSakuraCount + 1);

            // 一度に複数枚の桜を生成
            for(int i = 0; i < sakuraCount; i++)
            {
                // ランダムな桜を生成
                int randomIndex = Random.Range(0, sakuraPrefabs.Length);
                GameObject sakura = Instantiate(sakuraPrefabs[randomIndex], sakuraParent);

                // 初期位置をランダムに設定
                float randomX = Random.Range(spawnOffsetX.x, spawnOffsetX.y);
                float randomY = Random.Range(spawnOffsetY.x, spawnOffsetY.y);

                // 初期位置を設定
                sakura.GetComponent<RectTransform>().anchoredPosition = new Vector2(randomX, randomY);

                // リストに追加
                activeSakuraList.Add(sakura);

                // 桜の落下を制御
                StartCoroutine(FallSakura(sakura.GetComponent<RectTransform>()));
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator FallSakura(RectTransform sakura)
    {
        while(sakura != null && sakura.anchoredPosition.y > sakuraParent.rect.yMin)
        {
            // 落下 & ランダムな左右の動き
            float randomX = Random.Range(fallRangeX.x, fallRangeX.y);
            float randomY = Random.Range(fallRangeY.x, fallRangeY.y);

            sakura.anchoredPosition += new Vector2(randomX, randomY) * Time.deltaTime * fallSpeed;

            yield return null;
        }
        // 親領域を超えたら桜を削除
        if(sakura != null)
        {
            RemoveSakura(sakura.gameObject);
        }
    }

    private void ClearAllSakura()
    {
        // 現在生成されている桜を全て削除
        foreach (var sakura in activeSakuraList)
        {
            if(sakura != null)
            {
                Destroy(sakura);
            }
        }
        activeSakuraList.Clear();
    }

    private void RemoveSakura(GameObject sakura)
    {
        if(activeSakuraList.Contains(sakura))
        {
            activeSakuraList.Remove(sakura);
            Destroy(sakura);
        }
    }
}
