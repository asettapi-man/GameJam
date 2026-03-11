using System.Collections;
using UnityEngine;
using UnityEngine.UI;
//リザルト結果のアニメーションを順番に実行するスクリプト
public class ResultSequence : MonoBehaviour
{
    public float interval = 0.95f;

    [SerializeField] ResultAnimation[] animations;
    [SerializeField] Animator checkoutAnimation;
    [SerializeField] ScrollRect scrollRect; // ScrollView
    [SerializeField] bool debug;

    private bool isPlaying = false;

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
        int groupSize = 5;

        for (int i = 0; i < animations.Length; i += groupSize)
        {
            // 5個再生
            for (int j = i; j < i + groupSize && j < animations.Length; j++)
            {
                animations[j].Play();
            }

            // スクロールを少し下げる
            ScrollDown();

            yield return new WaitForSeconds(interval);
        }

        CheckoutAnimationPlay();
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
    }
}