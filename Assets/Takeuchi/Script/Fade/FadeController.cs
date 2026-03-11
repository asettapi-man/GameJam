using UnityEngine;
using System.Collections;

/// <summary>
/// フェードイン・フェードアウトのみを行うクラス
/// CanvasGroupのalphaを変更して画面を徐々に表示/非表示にする
/// </summary>
namespace Takeuchi
{
    public class FadeController : MonoBehaviour
    {
        // フェード対象のCanvasGroup
        // UI全体の透明度を管理する
        [SerializeField] private CanvasGroup canvasGroup;

        // フェードにかかる時間
        [SerializeField] private float fadeTime = 1f;

        /// <summary>
        /// フェードイン（透明 → 表示）
        /// </summary>
        public void FadeIn()
        {
            // コルーチンを開始
            StartCoroutine(Fade(0f, 1f));
        }

        /// <summary>
        /// フェードアウト（表示 → 透明）
        /// </summary>
        public void FadeOut()
        {
            // コルーチンを開始
            StartCoroutine(Fade(1f, 0f));
        }

        /// <summary>
        /// フェード処理
        /// alpha値を徐々に変更する
        /// </summary>
        IEnumerator Fade(float start, float end)
        {
            // 経過時間
            float time = 0f;

            // 指定時間になるまで繰り返す
            while (time < fadeTime)
            {
                // 経過時間を加算
                time += Time.deltaTime;

                // alphaを補間して変更
                canvasGroup.alpha = Mathf.Lerp(start, end, time / fadeTime);

                // 次のフレームまで待つ
                yield return null;
            }

            // 最終値を保証
            canvasGroup.alpha = end;
        }
    }
}