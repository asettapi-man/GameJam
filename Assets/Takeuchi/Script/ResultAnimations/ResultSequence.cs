using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//リザルト結果のアニメーションを順番に実行するスクリプト
public class ResultSequence : MonoBehaviour
{
    public float interval = 0.95f;
    [SerializeField] private float soundDelay = 0.3f;
    [SerializeField] private ResultAnimation[] animations;
    [SerializeField] private Animator checkoutAnimation;

    /// <summary>
    /// インスペクターで設定する
    /// Scroll View 
    /// </summary>
    [SerializeField] private ScrollRect scrollRect;
    
    //デバッグ用のブール
    [SerializeField] private bool debug; 
    /// <summary>
    /// インスペクターで設定する
    /// アニメーションを再生する度に鳴らすSE
    /// </summary>
    [SerializeField] private AudioSource audioSource;

    /// <summary>
    /// インスペクターで設定する
    /// 合計金額のアニメーションを再生する時に鳴らすSE
    /// </summary>
   [SerializeField] private AudioSource checkoutAudioSource;

    private bool isPlaying = false;
    //何個ずつアニメーションを再生するかの数
    [SerializeField] private　int groupSize = 4;
    void Start()
    {
        animations = GetComponentsInChildren<ResultAnimation>();
    }

    void Update()
    {
        if (debug && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlaySequence());
        }
    }

    IEnumerator PlaySequence()
    {

        for (int i = 0; i < animations.Length; i += groupSize)
        {
            // 4個再生
            for (int j = i; j < i + groupSize && j < animations.Length; j++)
            {
                animations[j].Play();
                StartCoroutine(PlaySoundDelay());
            }

            // スクロールを少し下げる
            ScrollDown();

            yield return new WaitForSeconds(interval);
        }

        CheckoutAnimationPlay();
    }

    IEnumerator PlaySoundDelay()
    {
        yield return new WaitForSeconds(soundDelay);
        audioSource.Play();
    }

    void ScrollDown()
    {
        // 少しずつ下に移動
        scrollRect.verticalNormalizedPosition -= 0.2f;

        // 0未満にならないようにする
        scrollRect.verticalNormalizedPosition =
            Mathf.Clamp01(scrollRect.verticalNormalizedPosition);
    }

    void CheckoutAnimationPlay()
    {
        checkoutAnimation.SetTrigger("Play");
        checkoutAudioSource.Play();
    }
}