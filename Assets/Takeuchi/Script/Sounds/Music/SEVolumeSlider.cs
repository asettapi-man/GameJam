using Takeuchi;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SE（効果音）音量を操作するスライダー
/// ・UIのSliderとSoundManagerのSE音量を同期する
/// ・スライダーを動かすことで効果音の音量をリアルタイムで変更できる
/// </summary>

/// SE用のスライダーにアタッチする
[RequireComponent(typeof(Slider))] // このスクリプトが付いているオブジェクトには必ずSliderが必要
public class SEVolumeSlider : MonoBehaviour
{
    // Sliderコンポーネントの参照
    Slider slider;

    void Start()
    {
        // 同じGameObjectに付いているSliderコンポーネントを取得
        slider = GetComponent<Slider>();

 // SoundManagerに保存されている現在のSE音量を取得してスライダーの初期値として設定する（ゲーム開始時の同期）
        slider.value = SoundManager.Instance.GetSEVolume();

 // スライダーの値が変更されたときに呼ばれるイベントを登録、スライダーを動かすとChangeVolume関数が呼ばれる
        slider.onValueChanged.AddListener(ChangeVolume);
    }

    /// <summary>
    /// スライダーの値が変更されたときに呼ばれる処理
    /// </summary>
    /// <param name="value">スライダーの現在の値（0～1）</param>
    void ChangeVolume(float value)
    {
  // SoundManagerにSE音量を設定、これによりゲーム内の効果音の音量が変更される
        SoundManager.Instance.SetSEVolume(value);
    }
}