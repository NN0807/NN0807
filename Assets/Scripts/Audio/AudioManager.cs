using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.SceneManagement;
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// 全体のボリューム値
    /// </summary>
    public const float ALL_VOLUME_VALUE = 0.015f;

    /// <summary>
    /// 最大同時再生可能なオーディオソースの数
    /// </summary>
    public const int maxSEAudioSources = 30;
    public const int maxBGMAudioSources = 2;

    /// <summary>
    /// BGM用のオーディオソース
    /// </summary>
    private List<AudioSource> bgmAudioSources = new List<AudioSource>();
    /// <summary>
    /// SE用のオーディオソース
    /// </summary>
    private List<AudioSource> seAudioSources = new List<AudioSource>();

    // オーディオクリップリスト
    // BGM
    private Dictionary<string, AudioClip> BGMClipDict = new Dictionary<string, AudioClip>();
    // SE
    private Dictionary<string, AudioClip> SEClipDict = new Dictionary<string, AudioClip>();

    // インスタンス
    public static AudioManager instance;

    //起動時に実行される
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize()
    {
        GameObject audioManager = new GameObject("AudioManager");
        audioManager.AddComponent<AudioManager>();
    }

    private void Awake()
    {
        // シングルトンの初期化
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // シーン遷移後も保持
        }

        // オーディオクリップ読み込み
        BGMClipDict = Resources.LoadAll<AudioClip>("BGM").ToDictionary(clip => clip.name, clip => clip);
        SEClipDict = Resources.LoadAll<AudioClip>("SE").ToDictionary(clip => clip.name, clip => clip);

        // オーディオソースの初期化
        for(int i = 0; i < maxBGMAudioSources; i++)
        {
            // BGM用のオーディオソース作成
            AudioSource bgmAudioSource = gameObject.AddComponent<AudioSource>();
            bgmAudioSource.playOnAwake = false;
            bgmAudioSources.Add(bgmAudioSource);
        }
        for(int i = 0; i < maxSEAudioSources; i++)
        {
            // SE用のオーディオソース作成
            AudioSource seAudioSource = gameObject.AddComponent<AudioSource>();
            seAudioSource.playOnAwake = false;
            seAudioSource.loop = false;
            seAudioSources.Add(seAudioSource);
        }

        // シーン遷移後の処理
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // オブジェクトが破棄されるときに解除
    }

    // シーンがロードされた後に処理される
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // オーディオを全停止
        StopAllBGM();
        StopAllSE();
    }

    /// <summary>
    /// 再生
    /// </summary>
    /// <param name="audioPath">オーディオのパス</param>
    /// <param name="volumeRate">音量の倍率</param>
    /// <param name="delay">再生されるまでの時間</param>
    /// <param name="pitch">ピッチ</param>
    /// <param name="isLoop">ループ再生指せるか</param>
    public void Play(
        string audioPath,
        float volumeRate = 1f,
        float delay = 0f,
        float pitch = 1f,
        bool isLoop = false)
    {
        // キーを取得
        string key = Path.GetFileNameWithoutExtension(audioPath);

        // SE辞書に該当するキーがあれば
        if (SEClipDict.TryGetValue(key, out AudioClip seClip))
        {
            // 再生中でないAudioSourceを検索
            AudioSource availableSource = seAudioSources.Find(source => !source.isPlaying);
            if (availableSource != null)
            {
                availableSource.clip = seClip;          // 再生するクリップを設定
                availableSource.clip = seClip;          // 再生するクリップを設定
                availableSource.volume =  volumeRate;   // 音量を設定
                availableSource.pitch = pitch;          // ピッチ
                availableSource.loop = isLoop;          // ループ設定
                availableSource.PlayDelayed(delay);     // 再生開始
            }
        }   
        else if(BGMClipDict.TryGetValue(key, out AudioClip bgmClip))
        {
            // 再生中でないAudioSourceを検索
            AudioSource availableSource = bgmAudioSources.Find(source => !source.isPlaying);
            if (availableSource != null)
            {
                availableSource.clip = bgmClip;         // 再生するクリップを設定
                availableSource.clip = bgmClip;         // 再生するクリップを設定
                availableSource.volume = volumeRate;    // 音量を設定
                availableSource.pitch = pitch;          // ピッチ
                availableSource.loop = isLoop;          // ループ設定
                availableSource.PlayDelayed(delay);     // 再生開始
            }
        }
    }

    /// <summary>
    /// 再生(Start関数用)
    /// </summary>
    /// <param name="audioPath">オーディオのパス</param>
    /// <param name="volumeRate">音量の倍率</param>
    /// <param name="delay">再生されるまでの時間</param>
    /// <param name="pitch">ピッチ</param>
    /// <param name="isLoop">ループ再生指せるか</param>
    public IEnumerator StartFuncPlay(
        string audioPath,
        float volumeRate = 1f,
        float delay = 0f,
        float pitch = 1f,
        bool isLoop = false)
    {
        yield return null; // 1フレーム待つ

        // キーを取得
        string key = Path.GetFileNameWithoutExtension(audioPath);

        // SE辞書に該当するキーがあれば
        if (SEClipDict.TryGetValue(key, out AudioClip seClip))
        {
            // 再生中でないAudioSourceを検索
            AudioSource availableSource = seAudioSources.Find(source => !source.isPlaying);
            if (availableSource != null)
            {
                availableSource.clip = seClip;          // 再生するクリップを設定
                availableSource.clip = seClip;          // 再生するクリップを設定
                availableSource.volume =  volumeRate;   // 音量を設定
                availableSource.pitch = pitch;          // ピッチ
                availableSource.loop = isLoop;          // ループ設定
                availableSource.PlayDelayed(delay);     // 再生開始
            }
        }   
        else if(BGMClipDict.TryGetValue(key, out AudioClip bgmClip))
        {
            // 再生中でないAudioSourceを検索
            AudioSource availableSource = bgmAudioSources.Find(source => !source.isPlaying);
            if (availableSource != null)
            {
                availableSource.clip = bgmClip;         // 再生するクリップを設定
                availableSource.clip = bgmClip;         // 再生するクリップを設定
                availableSource.volume = volumeRate;    // 音量を設定
                availableSource.pitch = pitch;          // ピッチ
                availableSource.loop = isLoop;          // ループ設定
                availableSource.PlayDelayed(delay);     // 再生開始
            }
        }
    }

    /// <summary>
    /// 全ての音量を変更
    /// </summary>
    /// <param name="volumRate"></param>
    public void ChangeAllVolumRate(float volumRate)
    {
        ChangeAllBGMVolume(volumRate);
        ChangeAllSEVolume(volumRate);
    }

    /// <summary>
    /// 再生中の全てのBGMを停止
    /// </summary>
    public void StopAllBGM()
    {
        // 再生中のものを検索
        List<AudioSource> availableSources = bgmAudioSources.FindAll(source => source.isPlaying);
        foreach(AudioSource source in availableSources)
        {
            if(source != null)
            {
                if (source.isPlaying) source.Stop();
            }
        }
    }

    /// <summary>
    /// 指定のBGMを停止
    /// </summary>
    /// <param name="audioPath">停止したいオーディオパス</param>
    public void StopBGM(string audioPath)
    {
        // クリップ名を取得
        string clipName= Path.GetFileNameWithoutExtension(audioPath);
        foreach(AudioSource source in bgmAudioSources)
        {
            // 指定されたクリップを再生中か確認
            if (source.isPlaying && source.clip.name == clipName) 
            {
                // 再生を停止
                source.Stop(); 
                break;
            }
        }
    }

    /// <summary>
    /// 再生中の全てのBGMを一時停止
    /// </summary>
    public void PauseAllBGM()
    {
        // 再生中のものを検索
        List<AudioSource> availableSources = bgmAudioSources.FindAll(source => source.isPlaying);
        foreach (AudioSource source in availableSources)
        {
            if (source != null)
            {
                if (source.isPlaying) source.Pause();
            }
        }
    }

    /// <summary>
    /// 一時停止中の全てのBGMを再生
    /// </summary>
    public void UnPauseAllBGM()
    {
        foreach (var source in bgmAudioSources) // 全てのBGM用AudioSourceを確認
        {
            if (!source.isPlaying && source.clip != null) // 再生が停止中か確認
            {
                source.UnPause(); // 再開
            }
        }
    }

    /// <summary>
    /// 指定のBGMを一時停止
    /// </summary>
    /// <param name="audioPath">一時停止したいオーディオパス</param>
    public void PauseBGM(string audioPath)
    {
        // クリップ名を取得
        string clipName = Path.GetFileNameWithoutExtension(audioPath);
        foreach (AudioSource source in bgmAudioSources)
        {
            // 指定されたクリップを再生中か確認
            if (source.isPlaying && source.clip.name == clipName)
            {
                // 再生を一時停止
                source.Pause();
                break;
            }
        }
    }

    /// <summary>
    /// 指定の一時停止中BGMを再開
    /// </summary>
    /// <param name="audioPath">再開したいオーディオパス</param>
    public void UnPauseBGM(string audioPath)
    {
        // クリップ名を取得
        string clipName = Path.GetFileNameWithoutExtension(audioPath);
        foreach (AudioSource source in bgmAudioSources)
        {
            // 指定されたクリップを停止中か確認
            if (!source.isPlaying && source.clip.name == clipName)
            {
                // 再開
                source.UnPause();
                break;
            }
        }
    }

    /// <summary>
    /// 全てのBGMの音量を変更
    /// </summary>
    /// <param name="volumRate">変更後の音量割合(0~1)</param>
    public void ChangeAllBGMVolume(float volumRate)
    {
        foreach (AudioSource source in bgmAudioSources)
        {
            source.volume = 1 * volumRate;
        }
    }

    /// <summary>
    /// 再生中の全てのSEを停止
    /// </summary>
    public void StopAllSE()
    {
        // 再生中のものを検索
        List<AudioSource> availableSources = seAudioSources.FindAll(source => source.isPlaying);
        foreach(AudioSource source in availableSources)
        {
            if(source != null)
            {
                if (source.isPlaying) source.Stop();
            }
        }
    }

    /// <summary>
    /// 指定のSEを停止
    /// </summary>
    /// <param name="audioPath">停止したいオーディオパス</param>
    public void StopSE(string audioPath)
    {
        // クリップ名を取得
        string clipName= Path.GetFileNameWithoutExtension(audioPath);
        foreach(AudioSource source in seAudioSources)
        {
            // 指定されたクリップを再生中か確認
            if (source.isPlaying && source.clip.name == clipName) 
            {
                // 再生を停止
                source.Stop(); 
                break;
            }
        }
    }

    /// <summary>
    /// 再生中の全てのSEを一時停止
    /// </summary>
    public void PauseAllSE()
    {
        // 再生中のものを検索
        List<AudioSource> availableSources = seAudioSources.FindAll(source => source.isPlaying);
        foreach (AudioSource source in availableSources)
        {
            if (source != null)
            {
                if (source.isPlaying) source.Pause();
            }
        }
    }

    /// <summary>
    /// 一時停止中の全てのSEを再生
    /// </summary>
    private void UnPauseAllSE()
    {
        foreach (var source in seAudioSources) // 全てのBGM用AudioSourceを確認
        {
            if (!source.isPlaying && source.clip != null) // 再生が停止中か確認
            {
                source.UnPause(); // 再開
            }
        }
    }

    /// <summary>
    /// 指定のSEを一時停止
    /// </summary>
    /// <param name="audioPath">一時停止したいオーディオパス</param>
    public void PauseSE(string audioPath)
    {
        // クリップ名を取得
        string clipName = Path.GetFileNameWithoutExtension(audioPath);
        foreach (AudioSource source in seAudioSources)
        {
            // 指定されたクリップを再生中か確認
            if (source.isPlaying && source.clip.name == clipName)
            {
                // 再生を一時停止
                source.Pause();
                break;
            }
        }
    }

    /// <summary>
    /// 指定の一時停止中SEを再開
    /// </summary>
    /// <param name="audioPath">再開したいオーディオパス</param>
    public void UnPauseSE(string audioPath)
    {
        // クリップ名を取得
        string clipName = Path.GetFileNameWithoutExtension(audioPath);
        foreach (AudioSource source in seAudioSources)
        {
            // 指定されたクリップを停止中か確認
            if (!source.isPlaying && source.clip.name == clipName)
            {
                // 再開
                source.UnPause();
                break;
            }
        }
    }

    /// <summary>
    /// 全てのSEの音量を変更
    /// </summary>
    /// <param name="volumRate">変更後の音量割合(0~1)</param>
    public void ChangeAllSEVolume(float volumRate)
    {
        foreach (AudioSource source in seAudioSources)
        {
            source.volume = 1 * volumRate;
        }
    }
}
