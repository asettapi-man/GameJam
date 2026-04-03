using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// マウスホイール入力を検知してオブジェクトを左右に移動させるスクリプト
/// 終点（EndPoint）または予測点（PredictionPoint）にアタッチして使用
/// 移動時に放物線の再描画と終点位置の更新を行う
/// </summary>
public class MouseWheelHorizontalMove : MonoBehaviour
{
    // X軸方向の移動速度倍率
    [SerializeField,Header("ホイール操作時の移動倍率"),Range(0,0.1f)]
    private float moveSpeed = 0.01f;


    [SerializeField, Header("X座標の最小値")]
    private float minX = -2.4f;

    [SerializeField, Header("X座標の最大値")]
    private float maxX = 5.4f;

    //スマホ用
    [SerializeField, Tooltip("スワイプと判定する最低距離")]
    private float swipeThreshold = 0.5f;

    //画面タップした開始位置
    private Vector2 startPos;

    //タップ中か確認フラグ
    private bool isTouching;

    /// <summary>
    /// 毎フレーム実行される処理
    /// マウスホイールの入力を監視
    /// </summary>
    void Update()
    {
        //ゲームオーバー時以外は実行
        if (GameManager.Instance.isGameOver) return;

        //== マウス処理 ==
        // マウスホイールのスクロール量を取得
        Vector2 scroll = Mouse.current.scroll.ReadValue();

        float scrollAmount = scroll.y;

        // 入力があった時のみ移動処理を実行
        if (scrollAmount != 0 && !LineManager.Instance.isMoving && !PauseManager.Instance.isPause)
        {
            MoveObject(scrollAmount);
        }

        //== スワイプ処理 ==
        //画面タップされたか？
        var primaryTouch = Touchscreen.current?.primaryTouch;
        if (primaryTouch != null) return;

        //タッチ開始
        if (primaryTouch.press.wasPressedThisFrame)
        {
            //開始位置を設定
            startPos = primaryTouch.position.ReadValue();
            isTouching = true; //タッチ中
        }

        //タッチ終了後にスワイプ判定
        if (primaryTouch.press.wasReleasedThisFrame && LineManager.Instance.isMoving && !PauseManager.Instance.isPause && isTouching)
        {
            //終点位置を設定
            Vector2 endPos = primaryTouch.position.ReadValue();

            //移動距離を計算
            Vector2 delta = endPos - startPos;

            //最低距離よりも長いか？
            if (delta.magnitude >= swipeThreshold)
            {
                MoveObject(delta.x);
            }
        }
    }

    /// <summary>
    /// オブジェクトをX軸方向に移動させる処理
    /// 移動後に放物線の再描画と終点位置の更新を実行
    /// </summary>
    /// <param name="input">マウスホイールの入力値（正：上回転、負：下回転）</param>
    void MoveObject(float input)
    {
        // 現在位置を取得
        Vector3 pos = transform.position;

        // X座標を入力量×速度分だけ移動
        pos.x += input * moveSpeed;

        // X座標の範囲制限
        pos.x = Mathf.Clamp(pos.x, minX, maxX);

        // 位置を更新
        transform.position = pos;

        // 放物線を再描画
        LineManager.Instance.UpadateLine();

        // 終点の位置を更新
        LineManager.Instance.UpdateEndPointsPos(transform);
    }
}
