using UnityEngine;

/// <summary>
/// 投げられるアイテムを管理するクラス
/// 生成時に終点と予測点を作成し、放物線描画を開始
/// 飛行終了時は物理演算を有効化し、放物線を削除
/// </summary>
public class FlyingItem : MonoBehaviour
{
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
    [SerializeField, Header("(暫定版)アイテム画像")]
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

        // 予測点の見た目を設定
        SpriteRenderer spriteRenderer = preObj.GetComponent<SpriteRenderer>();　//スプライトレンダラーの取得
        spriteRenderer.sprite = itemSprite;                                     //スプライトを適応
        spriteRenderer.color = predictionPointColor;                            //色を適応

        //Debug.Log("予測地点生成");

        // LineManagerに3点を登録して放物線描画開始
        LineManager.Instance.SetFlyingPoints(this.gameObject, endObj,preObj);
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
    }

}
