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

    /// <summary>
    /// 毎フレーム実行される処理
    /// マウスホイールの入力を監視
    /// </summary>
    void Update()
    {
        //ゲームオーバー時以外は実行
        if (GameManager.Instance.isGameOver) return;

        // マウスホイールのスクロール量を取得
        Vector2 scroll = Mouse.current.scroll.ReadValue();

        float scrollAmount = scroll.y;

        // 入力があった時のみ移動処理を実行
        if (scrollAmount != 0 && !LineManager.Instance.isMoving && !PauseManager.Instance.isPause)
        {
            MoveObject(scrollAmount);
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
