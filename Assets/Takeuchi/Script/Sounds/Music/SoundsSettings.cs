using UnityEngine;

/// <summary>
/// ゲーム設定保存クラス
/// ・音量
/// ・将来的にグラフィック設定なども追加可能
/// </summary>
public static class SoundsSettings
{
    /* ========================
       保存キー
    ======================== */

    const string MASTER_VOLUME = "MASTER_VOLUME";
    const string BGM_VOLUME = "BGM_VOLUME";
    const string SE_VOLUME = "SE_VOLUME";

    /* ========================
       音量保存
    ======================== */

    public static void SaveVolume(float master, float bgm, float se)
    {
        PlayerPrefs.SetFloat(MASTER_VOLUME, master);
        PlayerPrefs.SetFloat(BGM_VOLUME, bgm);
        PlayerPrefs.SetFloat(SE_VOLUME, se);

        PlayerPrefs.Save(); // 即保存
    }

    /* ========================
       音量ロード
    ======================== */

    public static float LoadMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
    }

    public static float LoadBGMVolume()
    {
        return PlayerPrefs.GetFloat(BGM_VOLUME, 1f);
    }

    public static float LoadSEVolume()
    {
        return PlayerPrefs.GetFloat(SE_VOLUME, 1f);
    }

    /* ========================
       設定リセット
    ======================== */

    public static void ResetSettings()
    {
        PlayerPrefs.DeleteKey(MASTER_VOLUME);
        PlayerPrefs.DeleteKey(BGM_VOLUME);
        PlayerPrefs.DeleteKey(SE_VOLUME);
    }
}