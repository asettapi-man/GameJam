using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Unityroom.Client;
using static ItemDataBase;

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

    //リザルト画面の表示フラグ
    private bool isResult = false;

    private UnityroomClient unityroomClient = new()
    {
        HmacKey = "JVkv9jPPm/ADPGPCgH8uBcvfUWkyJ1rBBRHvRpCTBDkDvVFRsR8lNn49+oGCOsujzgkJ3fNjTTp5iQoTkKmTEA=="
    };
    
    private void Start()
    {
        resultTexts = GetComponentsInChildren<ResultText>();
        resultImages = GetComponentsInChildren<ChangeResultImage>();
        switchingActiveImages = GetComponentsInChildren<SwitchingActiveImage>();
        // デバッグ用
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("vanillaIceCream"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("chocolateIceCream"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("chocolateIceCream"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("cottonCandyIceCream"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("chocolateMintIceCream"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("rabbitApple"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("marshmallow"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("marshmallow"));
        //GameManager.Instance.itemDatas.Add(itemDataBase.GetItemData("marshmallow"));
        //デバッグ用
        //UpdateResult();
        isResult = false;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameOver && !isResult)
        {
            //合計金額表示
            UpdateResult();
        }
    }

    /// <summary>
    /// アイテムデータベースのデータ数の取得
    /// </summary>
    /// <returns></returns>
    public int GetItemDataCount()
    {
        //アイテムデータベースがない　または　アイテムデータの数が０以下か？
        return itemDataBase == null || itemDataBase.datas.Count <= 0 ? 0 : itemDataBase.datas.Count;
    }

    //各アイテムの個数を数える関数
    public void DatasCount()
    {
        for (int i = 0; i < itemDataBase.datas.Count; i++)
        {
            ItemData data = itemDataBase.datas[i];

            int counter = itemDataBase.GetItemDataCount(data.itemName, GameManager.Instance.itemDatas);

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
    public async void UpdateResult()
    {
        DatasCount();
        ResultImageHidden();
        for (int i = 0; i < itemDataBase.datas.Count; i++)
        {
            ItemData data = itemDataBase.datas[i];
            // 個数取得
            int count = itemDataBase.GetItemDataCount(data.itemName, GameManager.Instance.itemDatas);

            //合計金額の取得
            int amount = itemDataBase.GetTotalAmount(GameManager.Instance.itemDatas);

            //合計金額を文字にする
            string amounts = amount.ToString();

            // テキスト更新
            resultTexts[i].SetText(count);

            // 画像更新
            resultImages[i].SetSprite(data.sprite);

            //合計金額を合計金額テキストに同期
            totalAmountText.text = amounts + "えん";
        }

        //スコアをUnityroomでも反映させる
        int totalAmount = itemDataBase.GetTotalAmount(GameManager.Instance.itemDatas);
        await unityroomClient.Scoreboards.SendAsync(new()
        {
            ScoreboardId = 1,
            Score = totalAmount
        });

        isResult = true;
    }
}