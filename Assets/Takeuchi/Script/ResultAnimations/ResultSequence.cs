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

    private ResultManegr result;

    void Start()
    {
        animations = GetComponentsInChildren<ResultAnimation>();
        result = GetComponentInChildren<ResultManegr>();
    }

    void Update()
    {
        if (GameManager.Instance.isGameOver && !isPlaying)
        {
            isPlaying = true;
            StartCoroutine(PlaySequence());
        }
    }

    IEnumerator PlaySequence()
    {
        var playCount = result.GetItemDataCount();

        for (int i = 0; i < playCount; i += groupSize)
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