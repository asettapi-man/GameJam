using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Item / ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [System.Serializable]
    public class ItemData
    {
        [Tooltip("投擲物名")]         public string itemName;
        [Tooltip("お金（スコア用）")] public int money;
        [Tooltip("重さ")]             public float gravity;
        [Tooltip("画像")]             public Sprite sprite;
    }

    [Tooltip("投擲物の情報データベース")]
    public List<ItemData> datas;

    public ItemData GetItemData(string itemName)
    {
        var data = datas.Find(data => data.itemName == itemName);

        //データがあれば、そのデータを返す
        return data != null ? data : null;
    }



    /// <summary>
    /// 投擲物名別合計金額の取得
    /// </summary>
    /// <param name="itemName">投擲物名</param>
    /// <param name="itemDatas">集計したアイテムのデータ</param>
    /// <returns>名前別の合計金額を返す</returns>
    public int GetTotalAmountByItem(string itemName, List<ItemData> itemDatas)
    {
        //合計金額用
        int totalAmount = 0;

        //カップ内にあるデータを全て確認
        foreach (var data in itemDatas)
        {
            //名前不一致なら処理飛ばす
            if (data.itemName != itemName) continue;

            //一致している名前があるデータが持っているお金を加算
            totalAmount += data.money;
        }

        //合計金額を返す
        return totalAmount;
    }

    /// <summary>
    /// 合計金額の取得
    /// </summary>
    /// <param name="itemDatas">集計したアイテムのデータ</param>
    /// <returns>合計金額を返す</returns>
    public int GetTotalAmount(List<ItemData> itemDatas)
    {
        //合計金額用
        int totalAmount = 0;

        foreach (var data in datas)
        {
            //それぞれのアイテムごとに集計
            string itemName = data.itemName;

            int amount = GetTotalAmountByItem(itemName, itemDatas);
            totalAmount += amount;
        }

        //合計金額を返す
        return totalAmount;
    }
}
