using UnityEngine;

/// <summary>
/// AudioSourceに音の種類を持たせる
/// </summary>
/// AudioSourceがアタッチされているゲームオブジェクトにアタッチ
/// SoundTypeをBGMかSEかを指定する
[RequireComponent(typeof(AudioSource))]
public class SoundSource : MonoBehaviour
{
    // 音の種類
    public SoundType type;

    // AudioSource
    public AudioSource audioSource;

    void Awake()
    {
        // AudioSource取得
        audioSource = GetComponent<AudioSource>();
    }
}