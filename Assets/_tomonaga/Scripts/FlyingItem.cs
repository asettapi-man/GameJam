using UnityEngine;
using System.Collections;
using UnityEditor.Rendering;

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

    // 生成した終点オブジェクトへの参照
    private GameObject endObj;

    // 生成した予測点オブジェクトへの参照
    private GameObject preObj;

    //
    public ParticleSystem shootingEffect;

    private AudioSource audioSource;

    private float interval = 1.0f;



    /// <summary>
    /// 初期化処理
    /// 終点と予測点を生成し、放物線描画を開始
    /// </summary>
    void Start()
    {
        //終点と予測点の初期位置を取得
        Vector3 initialEndPos = LineManager.Instance.GetinitialPosition();

        // 終点と予測点のオブジェクトを生成
        endObj = Instantiate(endPointPrafabs, initialEndPos, Quaternion.identity);
        preObj = Instantiate(predictionPointPrafabs, initialEndPos, Quaternion.identity);

        // LineManagerに3点を登録して放物線描画開始
        LineManager.Instance.SetFlyingPoints(this.gameObject, endObj,preObj);

        //スクリプトの同期
        imageChanges = GetComponent<ImageChanges>();

        audioSource = GameObject.FindWithTag("ThrowAudio").GetComponent<AudioSource>();
    }

    private void Update()
    {
        //ゲームオーバー時以外は実行
        if (GameManager.Instance.isGameOver) return;

        itemData = imageChanges.currentData;
        var scale = itemData.scale;

        // 予測点の見た目を設定
        if (preObj != null)
        {
            SpriteRenderer spriteRenderer = preObj.GetComponent<SpriteRenderer>();         //スプライトレンダラーの取得
            spriteRenderer.sprite = GetComponent<SpriteRenderer>().sprite;                  //スプライトを適応
            spriteRenderer.color = predictionPointColor;                                    //色を適応
            preObj.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale); //画像の大きさを適応
        }
    }

    /// <summary>
    /// 飛行終了時の処理
    /// 放物線移動を停止し、物理演算を有効化
    /// 終点と予測点のオブジェクトを削除
    /// </summary>
    public void FinishFlying()
    {
        StartCoroutine(GetInterval(interval));
    }

    private IEnumerator GetInterval(float interval)
    {
        // 放物線移動スクリプトを削除
        Destroy(GetComponent<MoveAlongLine>());

        // 物理演算を有効化（自由落下開始）
        gameObject.AddComponent<Rigidbody2D>();
        gameObject.AddComponent<PolygonCollider2D>();

        // 終点と予測点のオブジェクトを削除
        Destroy(endObj);
        Destroy(preObj);

        // LineManagerの参照をリセットし、放物線を非表示
        yield return new WaitForSeconds(interval);

        LineManager.Instance.ReSetFlyingPoints();

        // このスクリプト自体を削除
        Destroy(GetComponent<FlyingItem>());

        //投げたアイテムのデータを追加
        GameManager.Instance.itemDatas.Add(itemData);
        Debug.Log($"カップ内のアイテム数：{GameManager.Instance.itemDatas.Count}");

        shootingEffect.loop = false;
    }


    public void StartFlying()
    {
        shootingEffect.Play();
        audioSource.Play();
    }

}
