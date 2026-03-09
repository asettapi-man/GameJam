using UnityEngine;

public class LineManager:MonoBehaviour
{
    public static LineManager Instance { get; private set; }
    
    private MouseWheelHorizontalMove wheelHorizontalMove;

    private ParabolaLineRenderer lineRenderer;

    void Awake()
    {
        Instance = this;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_mouseWheelHorizontalMove"></param>
    public void SetWheelHorizontalMove(MouseWheelHorizontalMove _mouseWheelHorizontalMove)
    {
        wheelHorizontalMove = _mouseWheelHorizontalMove;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="_parabolaLineRenderer"></param>
    public void SetParabolaLineRenderer(ParabolaLineRenderer _parabolaLineRenderer)
    {
        lineRenderer = _parabolaLineRenderer;
    }

    public void UpadateLine()
    {
        lineRenderer.DrawParabola();
    }

}
