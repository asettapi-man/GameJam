using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("デバッグ用")]
    [Tooltip("データベース")]
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private Image image;
    [SerializeField] private string itemName;

    //カップに入っているアイテムのデータを管理（デバッグ）
    private List<ItemDataBase.ItemData> itemData = new List<ItemDataBase.ItemData> ();

    private void Awake()
    {
       for (int i = 0; i < 2; i++)
       {
           itemData.Add(new ItemDataBase.ItemData { itemName = "vanillaIceCream", money = 100, gravity = 1.0f, sprite = null });
           itemData.Add(new ItemDataBase.ItemData { itemName = "chocolateIceCream", money = 100, gravity = 1.0f, sprite = null });
        }

        int total = itemDataBase.GetTotalAmount(itemData);
        int counter = itemDataBase.GetItemDataCount(itemName, itemData);

        Debug.Log($"カップにあるアイテムの合計金額：{total}円");
        Debug.Log($"{itemName}の合計:{counter}");

        //画像の設定
        var data = itemDataBase.GetItemData(itemName);
        image.sprite = data.sprite;
    }
}
