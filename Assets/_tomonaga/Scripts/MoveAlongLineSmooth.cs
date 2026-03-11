using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 経路に沿ってオブジェクトを移動させるスクリプト
/// 初期速度から徐々に加速して最高速度に到達する
/// </summary>
public class MoveAlongLine : MonoBehaviour
{
    private LineRenderer line;

    // 初期速度
    [SerializeField, Header("初期速度"), Range(0, 10f)]
    private float initialSpeed = 1f;

    // 最高速度
    [SerializeField, Header("最高速度"), Range(0, 20f)]
    private float maxSpeed = 10f;

    // 最低速度（減速時の下限）
    [SerializeField, Header("最低速度"), Range(0, 5f)]
    private float minSpeed = 0.5f;

    // 下降時の加速度（毎秒どれだけ速度を増やすか）
    [SerializeField, Header("下降時の加速度"), Range(0, 30f)]
    private float descendAcceleration = 8f;

    // 上昇時の減速倍率（速度にこの値を掛ける）
    [SerializeField, Header("上昇時の減速倍率"), Range(0, 20f)]
    private float ascendDeceleration = 3f;

    // 現在の速度
    private float currentSpeed;

    // クリック入力アクション
    [SerializeField, Header("移動開始のボタン設定")]
    public InputAction clickAction;

    private Vector3[] points;       // LineRendererの頂点座標配列
    private int currentIndex = 0;   // 現在向かっている頂点のインデックス

    private FlyingItem flyingItem = null;
    private ImageChanges imageChanges = null;

    /// <summary>
    /// オブジェクトが有効になったとき
    /// </summary>
    void OnEnable()
    {
        //入力を有効化
        clickAction.Enable();
        clickAction.performed += OnClick;

        //ライン描画のリファレンスを取得する
        line = LineManager.Instance.GetParabolaLineRenderer().GetLineRenderer();

        //自オブジェクトから他コンポーネントの同期
        flyingItem = GetComponent<FlyingItem>();
        imageChanges = GetComponent<ImageChanges>();
    }

    /// <summary>
    /// オブジェクトが無効になったとき、入力アクションを解除（メモリリーク防止）
    /// </summary>
    void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    /// <summary>
    /// 毎フレーム実行
    /// </summary>
    void Update()
    {
        // 移動中のみ経路移動処理を実行
        if (LineManager.Instance.isMoving)
        {
            MoveAlongPath();
        }
    }

    /// <summary>
    /// クリック入力があったときに呼ばれるコールバック
    /// </summary>
    /// <param name="context">入力コンテキスト</param>
    void OnClick(InputAction.CallbackContext context)
    {
        //移動中でなければクリック処理実行
        if (!LineManager.Instance.isMoving)
        {
            StartMove();
        }
    }

    /// <summary>
    /// 移動を開始する
    /// </summary>
    void StartMove()
    {
        // 必要な参照が設定されているか確認
        if (line == null)
        {
            Debug.LogError("Lineが設定されていません");
            return;
        }

        // LineRendererの頂点が移動経路として十分か確認
        int count = line.positionCount;
        if (count < 2)
        {
            Debug.LogError("LineRendererの頂点が足りません");
            return;
        }

        // LineRendererから全頂点座標を取得
        points = new Vector3[count];
        line.GetPositions(points);

        // アイテムを始点に配置し、移動を開始
        transform.position = points[0];
        currentIndex = 0;
        currentSpeed = initialSpeed;  // 現在の速度を初期速度に設定
        LineManager.Instance.isMoving = true;
    }

    /// <summary>
    /// 経路に沿ってアイテムを移動させる（Updateから呼ばれる）
    /// </summary>
    void MoveAlongPath()
    {
        // 終点に到達したら移動終了
        if (currentIndex >= LineManager.Instance.PassConflictSegmentCount() - 1)
        {
            LineManager.Instance.isMoving = false;
            flyingItem.FinishFlying();
            return;
        }

        Vector3 start = points[currentIndex];
        Vector3 end = points[currentIndex + 1];

        // Y座標の変化を判定
        float yDifference = end.y - start.y;

        if (yDifference > 0.01f)
        {
            // 上昇中：減速
            currentSpeed = Mathf.Max(currentSpeed - ascendDeceleration * Time.deltaTime, minSpeed);
        }
        else if (yDifference < -0.01f)
        {
            // 下降中：加速
            currentSpeed = Mathf.Min(currentSpeed + descendAcceleration * Time.deltaTime, maxSpeed);
        }
        // それ以外（水平移動）は速度を維持

        // 現在の目標頂点へ現在の速度で移動
        transform.position = Vector3.MoveTowards(
            transform.position,
            end,
            currentSpeed * Time.deltaTime
        );

        // 目標頂点に十分近づいたら次の頂点へ進む
        if (Vector3.Distance(transform.position, end) < 0.01f)
        {
            currentIndex++;
        }
    }
}
