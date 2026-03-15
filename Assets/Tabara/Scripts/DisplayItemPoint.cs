using UnityEngine;
using UnityEngine.UI;

public class DisplayItemPoint : MonoBehaviour
{

    [SerializeField, Tooltip("プレハブ化したオブジェクト")]
    private GameObject prefabObj;

    [SerializeField, Tooltip("アイテムデータベース")]
    private ItemDataBase itemDataBase;

    //取得用
    private Image imageComponent;
    private Text textComponent;

    private void Awake()
    {
        imageComponent = prefabObj.gameObject.GetComponent<Image>();
        textComponent = prefabObj.gameObject.GetComponentInChildren<Text>();
        SetItemPoint();
    }

    /// <summary>
    /// アイテム別金額のUI表示
    /// </summary>
    private void SetItemPoint()
    {
        foreach (var data in itemDataBase.datas)
        {
            //アイテム別のデータ取得
            var score = data.money;
            var sprite = data.sprite;

            //コンポーネントの取得

            imageComponent.sprite = sprite;
            textComponent.text = $"{score.ToString()}えん";

            //UI生成
            var spawnObj = Instantiate(prefabObj);
            spawnObj.transform.SetParent(transform, false);
        }
    }
}
