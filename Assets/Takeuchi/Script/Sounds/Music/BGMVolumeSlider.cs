using Takeuchi;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// BGM音量を操作するスライダー
/// ・UIのSliderとSoundManagerのBGM音量を同期する
/// ・スライダーを動かすとBGM音量がリアルタイムで変更される
/// </summary>

/// BGM用のスライダーにアタッチする
[RequireComponent(typeof(Slider))] // このスクリプトを付けたオブジェクトに必ずSliderコンポーネントを付ける
public class BGMVolumeSlider : MonoBehaviour
{
    // スライダーの参照
    Slider slider;

    void Start()
    {
        // 同じオブジェクトについているSliderコンポーネントを取得
        slider = GetComponent<Slider>();

// SoundManagerに保存されている現在のBGM音量を取得し,スライダーの初期値として設定する（ゲーム起動時の同期）
        slider.value = SoundManager.Instance.GetBGMVolume();

// スライダーの値が変更されたときに呼ばれるイベントを登録,スライダーを動かすとChangeVolume関数が実行される
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    /// <summary>
    /// スライダーの値が変更されたときに呼ばれる
    /// </summary>
    /// <param name="value">スライダーの現在の値（0〜1）</param>
    void ChangeVolume(float value)
    {
        // SoundManagerにBGM音量を設定
        // これによりゲーム内のBGM音量が変更される
        SoundManager.Instance.SetBGMVolume(value);
    }
}