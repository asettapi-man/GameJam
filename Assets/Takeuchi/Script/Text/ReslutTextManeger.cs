using System.Collections.Generic;
using UnityEngine;
using static ItemDataBase;

/// <summary>
/// リザルトUI全体管理
/// ItemDataBaseを基にResultTextへ値を代入
/// </summary>
public class ReslutTextManeger : MonoBehaviour
{
    [Header("データベース")]
    [SerializeField] private ItemDataBase itemDataBase;

    [Header("UI(ResultText) 上から順番に対応")]
    [SerializeField] private ResultText[] resultTexts;

    // カップに入ったアイテム（ゲーム側から渡される想定）
    private List<ItemData> itemDatas = new List<ItemData>();


    /// <summary>
    /// リザルト更新
    /// </summary>
    public void UpdateResult()
    {
        for (int i = 0; i < resultTexts.Length; i++)
        {
            // データベースのアイテム名取得
            string itemName = itemDataBase.datas[i].itemName;

            // 個数取得
            int count = itemDataBase.GetItemDataCount(itemName, itemDatas);

            // UI更新
            resultTexts[i].SetText(count);
        }
    }
}