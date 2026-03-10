using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// マウスホイール入力を検知したら関数を呼び出し、
/// その入力量に応じてオブジェクトを左右に移動させる
/// </summary>
public class MouseWheelHorizontalMove : MonoBehaviour
{
    // 移動速度倍率
    public float moveSpeed = 0.01f;

    void Update()
    {
        // マウスホイールのスクロール量を取得
        Vector2 scroll = Mouse.current.scroll.ReadValue();

        float scrollAmount = scroll.y;

        // 入力があったときのみ関数を呼び出す
        if (scrollAmount != 0)
        {
            MoveObject(scrollAmount);
        }
    }

    /// <summary>
    /// オブジェクトを左右に移動させる関数
    /// </summary>
    /// <param name="input">ホイール入力値</param>
    void MoveObject(float input)
    {
        // 現在位置取得
        Vector3 pos = transform.position;

        // X方向に移動
        pos.x += input * moveSpeed;

        // 位置更新
        transform.position = pos;

        LineManager.Instance.UpadateLine();
        LineManager.Instance.UpdateEndPointsPos(transform);
    }
}
