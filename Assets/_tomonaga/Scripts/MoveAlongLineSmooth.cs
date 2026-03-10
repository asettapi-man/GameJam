using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 経路をたどってオブジェクトが移動していくスクリプト
/// </summary>
public class MoveAlongLine : MonoBehaviour
{
    private LineRenderer line;      // 移動経路となるLineRenderer

    // 移動速度
    [SerializeField,Header("移動速度"),Range(0,10f)]
    private float speed = 5f;

    // クリック入力アクション
    [SerializeField,Header("移動開始のボタンを設定")]
    private InputAction clickAction; 

    private Vector3[] points;       // LineRendererの頂点座標配列
    private int currentIndex = 0;   // 現在向かっている頂点のインデックス
    private bool isMoving = false;  // 移動中かどうか

    private FlyingItem flyingItem = null;

    // オブジェクトが有効になったとき
    void OnEnable()
    {
        //入力を有効か
        clickAction.Enable();
        clickAction.performed += OnClick;

        //ラインレンダラーのリファレンスを入手する
        line = LineManager.Instance.GetParabolaLineRenderer().GetLineRenderer();

        //同オブジェクトが持つFlyingItemコンポーネントの入手
        flyingItem = GetComponent<FlyingItem>();
    }

    // オブジェクトが無効になったとき、入力アクションを解除（メモリリーク防止）
    void OnDisable()
    {
        clickAction.performed -= OnClick;
        clickAction.Disable();
    }

    void Update()
    {
        // 移動中のみ経路移動処理を実行
        if (isMoving)
        {
            MoveAlongPath();
        }
    }

    // クリック入力があったとき呼ばれるコールバック
    void OnClick(InputAction.CallbackContext context)
    {
        StartMove();
    }

    // 移動を開始する
    void StartMove()
    {

        // 必要な参照が設定されているか確認
        if (line == null)
        {
            Debug.LogError("Lineが設定されていません");
            return;
        }

        // LineRendererの頂点数が移動経路として成立するか確認
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
        isMoving = true;
    }

    // 経路に沿ってアイテムを移動させる（Update毎に呼ばれる）
    void MoveAlongPath()
    {
        // 終点に到達したら移動を終了
        if (currentIndex >= LineManager.Instance.PassConflictSegmentCount() - 1)
        {
            isMoving = false;
            flyingItem.FinishFlying();
            return;
        }

        Vector3 start = points[currentIndex];
        Vector3 end = points[currentIndex + 1];

        // 現在の目標頂点へ一定速度で移動
        transform.position = Vector3.MoveTowards(
            transform.position,
            end,
            speed * Time.deltaTime
        );

        // 目標頂点に十分近づいたら次の頂点へ進む
        if (Vector3.Distance(transform.position, end) < 0.01f)
        {
            currentIndex++;
        }
    }
}