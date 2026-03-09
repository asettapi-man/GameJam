using UnityEngine;

public class LineManager:MonoBehaviour
{
    public static LineManager Instance { get; private set; }
    
    private ParabolaLineRenderer lineRenderer;

    void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// ラインレンダースクリプトのリファレンスを入手する
    /// </summary>
    /// <param name="_parabolaLineRenderer"></param>入手するリファレンス
    public void SetParabolaLineRenderer(ParabolaLineRenderer _parabolaLineRenderer)
    {
        lineRenderer = _parabolaLineRenderer;
    }

    /// <summary>
    /// ラインの更新
    /// </summary>
    public void UpadateLine()
    {
        lineRenderer.DrawParabola();
    }

}
