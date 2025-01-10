using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// シーン管理
using UnityEngine.SceneManagement;

using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PauseManager : MonoBehaviour
{

    // PostProcessing用のVolume
    [SerializeField] Volume     globalVolume;
    // プレイ中のUI
    [SerializeField] GameObject playCanvas;
    // メニューUI
    [SerializeField] GameObject menuCanvas;

    // Tabキーテクスチャ
    [SerializeField] GameObject tabKeyTexture;
    // メニューテクスチャ
    [SerializeField] GameObject menuTexture;

    // DepthOfField効果
    private DepthOfField depthOfField;
    // メニューがアクティブか
    private bool isPaused = false;

    // ポーズがアクティブかどうかを外部から読み取り可能に
    public bool IsPaused => isPaused;


    // Start is called before the first frame update
    void Start()
    {
        // DepthOfFieldエフェクトの取得
        globalVolume.profile.TryGet(out depthOfField);
        if (depthOfField == null)
            Debug.LogError("DepthOfField is not found in the global volume");

        // 初期状態
        SwitchCanvas(false);
        ShowTabKeyGuide(true);
    }

    // キャンバスの切り替え
    public void SwitchCanvas(bool showMenu)
    {
        isPaused = showMenu;

        playCanvas.SetActive(!showMenu);
        menuCanvas.SetActive(showMenu);
        SwitchDepthOfField(showMenu);

        // ポーズ中は時間を停止
        Time.timeScale = showMenu ? 0f : 1f;

        // TabキーUI非表示
        ShowTabKeyGuide(!showMenu);
    }

    public void SwitchDepthOfField(bool _switch)
    {
        if(depthOfField != null)
        {
            depthOfField.active = _switch;
        }
    }

    // Tabキー画像の表示切替
    private void ShowTabKeyGuide(bool show)
    {
        if(tabKeyTexture != null)
        {
            tabKeyTexture.SetActive(show);
        }
        if(menuTexture != null)
        {
            menuTexture.SetActive(show);
        }
    }

    // ボタンが押された時に呼び出される関数
    public void OnRestartButtonPressed()
    {
        SwitchCanvas(false);
    }

    public void OnCharacterSelectButtonPressed()
    {
        // キャラクター選択シーンへ移動
        Time.timeScale = 1f;
        // SceneManager.LoadScene("CharacterSelect");
    }

    public void OnQuitButtonPressed()
    {
        Debug.Log("Game Quit");
        // Application.Quit;
    }

    private void Update()
    {
        // Tabキーでポーズを切り替え
        if(Input.GetKeyDown(KeyCode.Tab) && !isPaused)
        {
            SwitchCanvas(true);
        }
    }
}
