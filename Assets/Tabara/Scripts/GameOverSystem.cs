using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField, Tooltip("リザルト画面")]
    private CanvasGroup resultObj;

    [SerializeField, Tooltip("リザルト画面表示までのカウントダウン")]
    private float interval = 1.0f;

    //リザルト画面に移るまでのカウントダウンの開始フラグ
    private bool isResult = false;

    private void Awake()
    {
        //初期化
        resultObj.alpha = 0;
        isResult = false;
        resultObj.blocksRaycasts = false;
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
            StartCoroutine(MoveResult(interval));
        }
    }

    /// <summary>
    /// リザルト画面表示処理
    /// </summary>
    /// <param name="interval">待つ秒数</param>
    /// <returns>数秒待ってからリザルト画面を表示する</returns>
    private IEnumerator MoveResult(float interval)
    {
        yield return new WaitForSeconds(interval);

        //表示と画面の再生停止
        resultObj.alpha = 1;
        resultObj.blocksRaycasts = true;
        GameManager.Instance.isGameOver = true;
    }
}
