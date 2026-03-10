using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineManager:MonoBehaviour
{
    public static LineManager Instance { get; private set; }
    
    private ParabolaLineRenderer lineRendererRef;

    public  GameObject FlyingItemPrafabs;

    private GameObject FlyingItem;

    private GameObject predictionPoint;

    public InputAction pushAction; // クリック入力アクション

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pushAction.Enable();
        pushAction.performed += OnClick;
    }


    /// <summary>
    /// ラインレンダースクリプトのリファレンスを入手する
    /// </summary>
    /// <param name="_parabolaLineRenderer"></param>入手するリファレンス
    public void SetParabolaLineRenderer(ParabolaLineRenderer _parabolaLineRenderer)
    {
        lineRendererRef = _parabolaLineRenderer;
    }

    /// <summary>
    /// ラインの更新
    /// </summary>
    public void UpadateLine()
    {
        lineRendererRef.DrawParabola();
    }

    void OnClick(InputAction.CallbackContext context)
    {
        Instantiate(FlyingItemPrafabs, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void SetFlyingPoints(GameObject _flyingItem,GameObject _predictionPoint)
    {
        lineRendererRef.GetFlyingPoints(_flyingItem, _predictionPoint);
        lineRendererRef.ActiveLine();
    }

}
