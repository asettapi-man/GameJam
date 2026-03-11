using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static ItemDataBase;

public class GameOverSystem : MonoBehaviour
{
    [SerializeField, Tooltip("リザルト画面")]
    private CanvasGroup resultObj;

    private void Awake()
    {
        //非表示
        resultObj.alpha = 0;
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
        Debug.Log($"お金：{data.money}");

        Debug.Log($"アイテムデータ数：{GameManager.Instance.itemDatas.Count}");

        //接触したオブジェクトを削除
        Destroy(collision.gameObject);

        //表示と画面の再生停止
        GameManager.Instance.isGameOver = true;
        resultObj.alpha = 1;
    }

    private 
}
