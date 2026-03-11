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
        [Tooltip("画像の大きさ")]     public float scale;
        [Tooltip("画像")]             public Sprite sprite;
        [Tooltip("効果音")]           public AudioClip clip;
    }

    [Tooltip("投擲物の情報データベース")]
    public List<ItemData> datas;

    /// <summary>
    /// 名前別アイテムデータの取得
    /// </summary>
    /// <param name="itemName">検索するアイテム名</param>
    /// <returns>一致した名前のデータを返す</returns>
    public ItemData GetItemData(string itemName)
    {
        var data = datas.Find(data => data.itemName == itemName);

        //データがあれば、そのデータを返す
        return data != null ? data : null;
    }

    /// <summary>
    /// アイテム別の個数の合計を取得
    /// </summary>
    /// <param name="itemName">投擲物名</param>
    /// <param name="itemDatas">取得したアイテムのデータ</param>
    /// <returns>名前別の合計個数を返す</returns>
    public int GetItemDataCount(string itemName, List<ItemData> itemDatas)
    {
        //合計個数用
        int counter = 0;

        //データの中身を全て確認
        foreach (var data in itemDatas)
        {
            //データと名前が一致しているか？
            if (itemName != data.itemName) continue;

            //一致してたらインクリメント
            counter++;
        }

        //合計個数を返す
        return counter;
    }

    /// <summary>
    /// 投擲物名別合計金額の取得
    /// </summary>
    /// <param name="itemName">投擲物名</param>
    /// <param name="itemDatas">集計したアイテムのデータ</param>
    /// <returns>名前別の合計金額を返す</returns>
    private int GetTotalAmountByItem(string itemName, List<ItemData> itemDatas)
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
