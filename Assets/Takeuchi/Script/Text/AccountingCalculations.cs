using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//合計金額をテキストに表示するスクリプト
public class AccountingCalculations : MonoBehaviour
{
    [Header("デバッグ用")]
    [Tooltip("データベース")]
    [SerializeField] private ItemDataBase itemDataBase;

    //カップに入っているアイテムのデータを管理（デバッグ）
    private List<ItemDataBase.ItemData> itemData = new List<ItemDataBase.ItemData>();

   public int totalAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            itemData.Add(new ItemDataBase.ItemData { itemName = "ice", money = 50, sprite = null });
            itemData.Add(new ItemDataBase.ItemData { itemName = "cake", money = 100, sprite = null });
        }

        totalAmount = itemDataBase.GetTotalAmount(itemData);
    }
}
