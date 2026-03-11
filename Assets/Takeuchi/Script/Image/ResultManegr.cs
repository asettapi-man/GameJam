using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemDataBase;
using static UnityEditor.Timeline.Actions.MenuPriority;

/// <summary>
/// リザルトUI(Image)管理
/// </summary>
public class ResultManegr : MonoBehaviour
{
    [Header("データベース")]
    [SerializeField] private ItemDataBase itemDataBase;

    [Header("個数表示")]
    [SerializeField] private ResultText[] resultTexts;

    [Header("画像表示")]
    [SerializeField] private ChangeResultImage[] resultImages;

    [Header("イメージのアクティブ設定")]
    [SerializeField] private SwitchingActiveImage[] switchingActiveImages;

    //インスペクターで指定する
    [Header("合計金額のテキスト")]
    [SerializeField] private Text totalAmountText;

    // カップに入ったアイテム
    private List<ItemData> itemDatas = new List<ItemData>();
    private void Start()
    {
        resultTexts = GetComponentsInChildren<ResultText>();
        resultImages = GetComponentsInChildren<ChangeResultImage>();
        switchingActiveImages = GetComponentsInChildren<SwitchingActiveImage>();
        // デバッグ用
        itemDatas.Add(itemDataBase.GetItemData("vanillaIceCream"));
        itemDatas.Add(itemDataBase.GetItemData("chocolateIceCream"));
        itemDatas.Add(itemDataBase.GetItemData("chocolateIceCream"));
        itemDatas.Add(itemDataBase.GetItemData("cottonCandyIceCream"));
        itemDatas.Add(itemDataBase.GetItemData("chocolateMintIceCream"));
        itemDatas.Add(itemDataBase.GetItemData("rabbitApple"));
        itemDatas.Add(itemDataBase.GetItemData("marshmallow"));
        itemDatas.Add(itemDataBase.GetItemData("marshmallow"));
        itemDatas.Add(itemDataBase.GetItemData("marshmallow"));
        //デバッグ用
        UpdateResult();
    }

    //各アイテムの個数を数える関数
    public void DatasCount()
    {
        for (int i = 0; i < itemDataBase.datas.Count; i++)
        {
            ItemData data = itemDataBase.datas[i];

            int counter = itemDataBase.GetItemDataCount(data.itemName, itemDatas);

            Debug.Log(data.itemName + " : " + counter);
        }
    }
    //必要なイメージのみを表示する関数
   public void ResultImageHidden()
    {
        for (int i = itemDataBase.datas.Count; i < switchingActiveImages.Length; i++)
        {
            switchingActiveImages[i].Hidden();
        }
    }
    /// <summary>
    /// リザルト更新
    /// </summary>
    public void UpdateResult()
    {
        DatasCount();
        ResultImageHidden();
        for (int i = 0; i < itemDataBase.datas.Count; i++)
        {
            ItemData data = itemDataBase.datas[i];
            // 個数取得
            int count = itemDataBase.GetItemDataCount(data.itemName, itemDatas);

            //合計金額の取得
            int amount = itemDataBase.GetTotalAmount(itemDatas);

            //合計金額を文字にする
            string amounts = amount.ToString();

            // テキスト更新
            resultTexts[i].SetText(count);

            // 画像更新
            resultImages[i].SetSprite(data.sprite);

            //合計金額を合計金額テキストに同期
            totalAmountText.text = amounts + "円";
        }
       
    }
}