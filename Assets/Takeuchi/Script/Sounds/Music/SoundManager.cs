using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// サウンド管理クラス（実務レベル）
/// ・Master / BGM / SE 音量管理
/// ・ミュート対応
/// ・PlayerPrefs保存
/// ・UIスライダー同期
/// ・SoundSource自動管理
/// </summary>
/// 空のゲームオブジェクトにアタッチする
public class SoundManager : MonoBehaviour
{
    /* ========================
       Singleton
    ======================== */

    public static SoundManager Instance;

    [System.Obsolete]
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンを跨いでも残す
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // SoundSource取得
        RegisterAllSources();

        // 保存データロード
        LoadVolume();

        // 初期音量適用
        ApplyVolume();
    }

    /* ========================
       SoundSource管理
    ======================== */

    private List<SoundSource> sources = new List<SoundSource>();

    [System.Obsolete]
    void RegisterAllSources()
    {
        sources.Clear();
        sources.AddRange(FindObjectsOfType<SoundSource>());
    }

    /* ========================
       音量パラメータ
    ======================== */

    float masterVolume = 1f;
    float bgmVolume = 1f;
    float seVolume = 1f;

    /* ========================
       ミュート保存用
    ======================== */

    float masterBeforeMute = 1f;
    float bgmBeforeMute = 1f;
    float seBeforeMute = 1f;

    /* ========================
       ミュート状態
    ======================== */

    bool masterMute;
    bool bgmMute;
    bool seMute;

    /* ========================
       UIスライダー
    ======================== */

    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider seSlider;

    /* ========================
       音量変更
    ======================== */

    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        SaveVolume();
        ApplyVolume();
    }

    public void SetBGMVolume(float value)
    {
        bgmVolume = value;
        SaveVolume();
        ApplyVolume();
    }

    public void SetSEVolume(float value)
    {
        seVolume = value;
        SaveVolume();
        ApplyVolume();
    }

    /* ========================
       音量適用
    ======================== */

    void ApplyVolume()
    {
        foreach (var s in sources)
        {
            if (s == null) continue;

            float volume = 1f;

            if (s.type == SoundType.BGM)
                volume = masterVolume * bgmVolume;

            if (s.type == SoundType.SE)
                volume = masterVolume * seVolume;

            if (masterMute) volume = 0;
            if (bgmMute && s.type == SoundType.BGM) volume = 0;
            if (seMute && s.type == SoundType.SE) volume = 0;

            s.audioSource.volume = volume;
        }
    }

    /* ========================
       ミュート
    ======================== */
    //マスター音量のミュートボタン用関数
    public void ToggleMasterMute()
    {
        masterMute = !masterMute;

        if (masterMute)
        {
            masterBeforeMute = masterVolume;
            masterVolume = 0;

            if (masterSlider)
                masterSlider.value = 0;
        }
        else
        {
            masterVolume = masterBeforeMute;

            if (masterSlider)
                masterSlider.value = masterVolume;
        }

        ApplyVolume();
    }
    //BGMのミュートボタン用関数
    public void ToggleBGMMute()
    {
        bgmMute = !bgmMute;

        if (bgmMute)
        {
            bgmBeforeMute = bgmVolume;
            bgmVolume = 0;

            if (bgmSlider)
                bgmSlider.value = 0;
        }
        else
        {
            bgmVolume = bgmBeforeMute;

            if (bgmSlider)
                bgmSlider.value = bgmVolume;
        }

        ApplyVolume();
    }
    //SEのミュートボタン用関数
    public void ToggleSEMute()
    {
        seMute = !seMute;

        if (seMute)
        {
            seBeforeMute = seVolume;
            seVolume = 0;

            if (seSlider)
                seSlider.SetValueWithoutNotify(0);
        }
        else
        {
            seVolume = seBeforeMute;

            if (seSlider)
                seSlider.SetValueWithoutNotify(seVolume);
        }

        ApplyVolume();
    }

    /* ========================
       PlayerPrefs保存
    ======================== */

    void SaveVolume()
    {
        PlayerPrefs.SetFloat("MASTER_VOLUME", masterVolume);
        PlayerPrefs.SetFloat("BGM_VOLUME", bgmVolume);
        PlayerPrefs.SetFloat("SE_VOLUME", seVolume);
    }

    void LoadVolume()
    {
        masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        seVolume = PlayerPrefs.GetFloat("SE_VOLUME", 1f);

        if (masterSlider) masterSlider.value = masterVolume;
        if (bgmSlider) bgmSlider.value = bgmVolume;
        if (seSlider) seSlider.value = seVolume;
    }

    /* ========================
       UI取得用
    ======================== */

    public float GetMasterVolume() => masterVolume;
    public float GetBGMVolume() => bgmVolume;
    public float GetSEVolume() => seVolume;
}