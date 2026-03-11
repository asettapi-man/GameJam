using UnityEngine;

[CreateAssetMenu(fileName = "AdjustMenu", menuName = "Scriptable Objects/AdjustMenu")]
public class AdjustMenu : ScriptableObject
{
    [Header("選択されたアイテムが飛んで行く速度"),Range(0, 10f)]
    public float _itemMoveSpeed = 5f;

    [Header("ホイールでの予測地点（厳密には終点）を移動倍率"), Range(0, 0.2f)]
    public float _endPointMoveSpeed = 0.1f;

    [Header("放物線の頂点の数（多いほどなめらか）"), Range(10, 100)]
    public int _lineSegmentCount = 30;

    [Header("放物線の高さ"), Range(1f, 10f)]
    public float _lineHeight = 2f;
}
