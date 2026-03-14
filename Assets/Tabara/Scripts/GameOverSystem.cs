using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField, Tooltip("リザルト画面")]
    private CanvasGroup resultObj;

    [Header("リザルト画面表示までのカウントダウン")]
    [SerializeField, Tooltip("カウントダウン表示画面")]
    private CanvasGroup countdownObj;
    [SerializeField, Tooltip("カウントダウン表示テキスト")]
    private Text countdownText;
    [SerializeField, Tooltip("カウントダウン")]
    private int countdownSeconds = 3;
    [SerializeField, Tooltip("表示終了までの時間")]
    private float finishMessageTextDuration = 1.0f;

    //リザルト画面に移るまでのカウントダウンの開始フラグ
    private bool isResult = false;

    private void Awake()
    {
        //初期化
        resultObj.alpha = 0.0f;
        countdownObj.alpha = 0.0f;
        resultObj.blocksRaycasts = false;
        countdownObj.blocksRaycasts = false;
        isResult = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //アイテムタグを持っているか？
        if (!collision.CompareTag("Item")) return;

        //接触したオブジェクトが持っているアイテムデータを取得
        var data = collision.GetComponent<ImageChanges>().currentData;

        //集計用アイテムデータリストから削除
        GameManager.Instance.itemDatas.Remove(data);
        Debug.Log($"削除したアイテムデータ");
        Debug.Log($"名前：{data.itemName}");

        Debug.Log($"アイテムデータ数：{GameManager.Instance.itemDatas.Count}");

        //接触したオブジェクトを削除
        Destroy(collision.gameObject);

        //カウントダウン開始
        if (!isResult)
        {
            isResult = true;
            StartCoroutine(CountDownToResult());
        }
    }

    /// <summary>
    /// カウントダウン表示
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountDownToResult()
    {
        //カウントダウン画面の表示
        countdownObj.alpha = 1.0f;
        countdownObj.blocksRaycasts = true;

        //カウントダウン用初期値
        float elapsed = 0f;
        int lastRemaining = -1;

        //表示時間まで表示する
        while (elapsed < countdownSeconds)
        {
            //自然数に変換
            int elapsedSeconds = (int)elapsed;
            int remaining = countdownSeconds - elapsedSeconds;

            if (lastRemaining != remaining)
            {
                //時間を表示
                countdownText.text = remaining.ToString();
                lastRemaining = remaining;
            }

            yield return null;
            elapsed += Time.unscaledDeltaTime;
        }

        //表示終了時
        countdownText.text = "終了！";
        yield return new WaitForSecondsRealtime(finishMessageTextDuration);

        countdownObj.alpha = 0.0f;
        DisplayResult();
    }

    /// <summary>
    /// リザルト画面の表示
    /// </summary>
    public void DisplayResult()
    {
        //表示と画面の再生停止
        resultObj.alpha = 1;
        resultObj.blocksRaycasts = true;
        GameManager.Instance.isGameOver = true;
    }
}
