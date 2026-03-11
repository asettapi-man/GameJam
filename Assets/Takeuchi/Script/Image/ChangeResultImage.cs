using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Imageの画像を変更を実行するスクリプト
public class ChangeResultImage : MonoBehaviour
{
    [Header("デバッグ用")]
    [Tooltip("データベース")]
    [SerializeField] private ItemDataBase itemDataBase;
    [SerializeField] private Image image;
    [SerializeField] private string itemName;

    //カップに入っているアイテムのデータを管理（デバッグ）
    private List<ItemDataBase.ItemData> itemData = new List<ItemDataBase.ItemData>();
    private void Start()
    {
        image = GetComponent<Image>();
        //画像の設定
        var data = itemDataBase.GetItemData(itemName);
        image.sprite = data.sprite;
    }
    
}
