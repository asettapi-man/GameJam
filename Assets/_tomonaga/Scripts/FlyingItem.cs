using UnityEngine;

/// <summary>
/// 投げられるアイテムを管理するクラス
/// 生成時に終点と予測点を作成し、放物線描画を開始
/// 飛行終了時は物理演算を有効化し、放物線を削除
/// </summary>
public class FlyingItem : MonoBehaviour
{
    //アイテムデータ１件分
    private ItemDataBase.ItemData itemData;

    private ImageChanges imageChanges;

    // 予測着地点のプレハブ
    [SerializeField,Header("予測地点用オブジェクト")]
    private GameObject predictionPointPrafabs;

    // マウスホイールで移動する終点のプレハブ
    [SerializeField, Header("終点用オブジェクト")]
    private GameObject endPointPrafabs;

    // 予測着地点の表示色
    [SerializeField, Header("予測地点用オブジェクト色")]
    private Color predictionPointColor;

    // アイテムに表示するスプライト
    private Sprite itemSprite;

    // 終点までの初期距離
    [SerializeField, Header("予測地点までの距離"), Range(0, 20)]
    private int predictionDistance;

    // 生成した終点オブジェクトへの参照
    private GameObject endObj;

    // 生成した予測点オブジェクトへの参照
    private GameObject preObj;


    /// <summary>
    /// 初期化処理
    /// 終点と予測点を生成し、放物線描画を開始
    /// </summary>
    void Start()
    {
        // 終点と予測点のオブジェクトを生成
        endObj = Instantiate(endPointPrafabs, new Vector3(predictionDistance, 0, 0), Quaternion.identity);
        preObj = Instantiate(predictionPointPrafabs, new Vector3(predictionDistance, 0, 0), Quaternion.identity);

        //Debug.Log("予測地点生成");

        // LineManagerに3点を登録して放物線描画開始
        LineManager.Instance.SetFlyingPoints(this.gameObject, endObj,preObj);

        //スクリプトの同期
        imageChanges = GetComponent<ImageChanges>();
    }

    private void Update()
    {
        itemData = imageChanges.currentData;
        var scale = itemData.scale;

        // 予測点の見た目を設定
        SpriteRenderer spriteRenderer = preObj.GetComponent<SpriteRenderer>();　//スプライトレンダラーの取得
        spriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;          //スプライトを適応
        spriteRenderer.color = predictionPointColor;                            //色を適応
        preObj.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale); //画像の大きさを適応
    }

    /// <summary>
    /// 飛行終了時の処理
    /// 放物線移動を停止し、物理演算を有効化
    /// 終点と予測点のオブジェクトを削除
    /// </summary>
    public void FinishFlying()
    {
        // 放物線移動スクリプトを削除
        Destroy(GetComponent<MoveAlongLine>());

        // 物理演算を有効化（自由落下開始）
        gameObject.AddComponent<Rigidbody2D>();
        gameObject.AddComponent<PolygonCollider2D>();

        // このスクリプト自体を削除
        Destroy(GetComponent<FlyingItem>());

        // LineManagerの参照をリセットし、放物線を非表示
        LineManager.Instance.ReSetFlyingPoints();

        // 終点と予測点のオブジェクトを削除
        Destroy(endObj);
        Destroy(preObj);

        //投げたアイテムのデータを追加
        GameManager.Instance.itemDatas.Add(itemData);
        Debug.Log($"カップ内のアイテム数：{GameManager.Instance.itemDatas.Count}");
    }

    /// <summary>
    /// アイテムデータの設定
    /// </summary>
    /// <param name="data"></param>
    public void SetItemData(ItemDataBase.ItemData data)
    {
        itemData = data;

        Debug.Log($"投げたアイテムのデータ");
        Debug.Log($"名前：{itemData.itemName}");
        Debug.Log($"お金：{itemData.money}");
        Debug.Log($"重さ：{itemData.gravity}");
    }
}
