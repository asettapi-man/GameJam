using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 放物線描画システム全体を管理するマネージャークラス
/// FlyingItemとParabolaLineRendererの橋渡し役を担う
/// </summary>
public class LineManager:MonoBehaviour
{
    // シングルトンインスタンス
    public static LineManager Instance { get; private set; }

    // 放物線描画を行うGameObjectへの参照
    private GameObject lineRendererRef;

    //上記オブジェクトが持つコンポーネントへの参照
    private ParabolaLineRenderer lineRendererComponentRef;

    // 投げるアイテムのプレハブ
    [SerializeField,Header("投げるアイテムのプレハブ")]
    private  GameObject FlyingItemPrafabs;

    // 現在飛行中のアイテム
    private GameObject FlyingItem;

    // 放物線の終点オブジェクト
    private GameObject EndPoint;

    // 予測着地点を表示するオブジェクト
    private GameObject predictionPoint;

    //投げるアイテムが移動中フラグ
    [HideInInspector]
    public bool isMoving = false;

    //予測点初期位置オブジェクト
    [SerializeField, Header("予測点の初期位置となるオブジェクト")]
    private Transform initialPredictionObject;

    /// <summary>
    /// シングルトンの初期化
    /// </summary>
    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 入力アクションの設定
    /// </summary>
    private void Start()
    {
        //新規作成
        if (FlyingItem == null)
        {
            Vector3 selfpos = transform.position;
            FlyingItem = Instantiate(FlyingItemPrafabs, selfpos, Quaternion.identity);
        }
    }


    /// <summary>
    /// ParabolaLineRendererを持つGameObjectの参照を設定
    /// </summary>
    /// <param name="_parabolaLineRenderer">放物線描画を行うGameObject</param>
    public void SetParabolaLineRenderer(GameObject _parabolaLineRenderer)
    {
        lineRendererRef = _parabolaLineRenderer;
        lineRendererComponentRef = lineRendererRef.GetComponent<ParabolaLineRenderer>();
    }

    /// <summary>
    /// ParabolaLineRendererコンポーネントを取得
    /// </summary>
    /// <returns>ParabolaLineRendererコンポーネント</returns>
    public ParabolaLineRenderer GetParabolaLineRenderer()
    {
        return lineRendererComponentRef;
    }


    /// <summary>
    /// 放物線の再描画を実行
    /// </summary>
    public void UpadateLine()
    {
        lineRendererComponentRef.ActiveLine();
    }

    /// <summary>
    /// 飛行に関わる3つのポイント（始点・終点・予測点）を設定し、放物線を描画開始
    /// </summary>
    /// <param name="_flyingItem">飛行するアイテム（始点）</param>
    /// <param name="_endPoint">マウスホイールで移動する終点</param>
    /// <param name="_predictionPoint">衝突判定後の実際の着地予測点</param>
    public void SetFlyingPoints(GameObject _flyingItem,GameObject _endPoint,GameObject _predictionPoint)
    {
        FlyingItem = _flyingItem;
        EndPoint = _endPoint;
        predictionPoint = _predictionPoint;

        lineRendererComponentRef.GetFlyingPoints(_flyingItem.transform, _endPoint.transform,_predictionPoint.transform);
        lineRendererRef.SetActive(true);
        lineRendererComponentRef.ActiveLine();
    }

    /// <summary>
    /// 飛行終了時に各ポイントの参照をリセットし、放物線を非表示にする
    /// </summary>
    public void ReSetFlyingPoints()
    {
        FlyingItem = null;
        EndPoint = null;
        predictionPoint = null;
        lineRendererRef.SetActive(false);

        //新規作成
        if (FlyingItem == null)
        {
            Vector3 selfpos = transform.position;
            FlyingItem = Instantiate(FlyingItemPrafabs, selfpos, Quaternion.identity);
        }
    }

    /// <summary>
    /// 終点の位置を更新（マウスホイール操作時に呼ばれる）
    /// </summary>
    /// <param name="endTransform">新しい終点のTransform</param>
    public void UpdateEndPointsPos(Transform endTransform)
    {
        lineRendererComponentRef.UpdateEndPointPos(endTransform);
    }

    /// <summary>
    /// 予測着地点の位置を更新（衝突判定後に呼ばれる）
    /// </summary>
    /// <param name="prepos">新しい予測着地点の座標</param>
    public void UpdatePredictionPointPos(Vector3 prepos)
    {
        predictionPoint.transform.position = prepos;
    }

    /// <summary>
    /// 衝突が発生した頂点番号を取得
    /// </summary>
    /// <returns>衝突した頂点のインデックス（衝突なしの場合はsegmentCount）</returns>
    public int PassConflictSegmentCount()
    {
        return lineRendererComponentRef.GetConflictSegmentCount();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetinitialPosition()
    {
        Vector3 initinalPosition = initialPredictionObject.transform.position;
        return initinalPosition;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void ClearParticleFunc()
    {
        lineRendererComponentRef.ClearParticles();
    }

}
