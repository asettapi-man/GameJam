using Takeuchi;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Master音量を操作するスライダー
/// ・ゲーム全体の音量（BGM・SEすべて）を調整する
/// ・UIのSliderとSoundManagerのMaster音量を同期する
/// </summary>

/// マスター音量用のスライダーにアタッチする
[RequireComponent(typeof(Slider))] // このスクリプトが付いているオブジェクトには必ずSliderが必要
public class MasterVolumeSlider : MonoBehaviour
{
    // UIスライダーの参照
    Slider slider;

    void Start()
    {
        // 同じGameObjectについているSliderコンポーネントを取得
        slider = GetComponent<Slider>();

 // SoundManagerに保存されている現在のMaster音量を取得し、スライダーの初期値として設定する（起動時に音量を同期）
        slider.value = SoundManager.Instance.GetMasterVolume();

// スライダーの値が変更されたときに呼ばれるイベントを登録、スライダーを動かすとChangeVolume関数が実行される
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    /// <summary>
    /// スライダーの値が変更されたときに呼ばれる
    /// </summary>
    /// <param name="value">スライダーの現在の値（0～1）</param>
    void ChangeVolume(float value)
    {
        // SoundManagerのMaster音量を変更
        // これによりゲーム内すべての音量に影響する
        SoundManager.Instance.SetMasterVolume(value);
    }
}