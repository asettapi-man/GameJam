using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class LineManager:MonoBehaviour
{
    public static LineManager Instance { get; private set; }
    
    private GameObject lineRendererRef;

    public  GameObject FlyingItemPrafabs;

    private GameObject FlyingItem;

    private GameObject EndPoint;

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
    public void SetParabolaLineRenderer(GameObject _parabolaLineRenderer)
    {
        lineRendererRef = _parabolaLineRenderer;
    }

    public ParabolaLineRenderer GetParabolaLineRenderer()
    {
        return lineRendererRef.GetComponent<ParabolaLineRenderer>();
    }
    

    /// <summary>
    /// ラインの更新
    /// </summary>
    public void UpadateLine()
    {
        lineRendererRef.GetComponent<ParabolaLineRenderer>().ActiveLine();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    void OnClick(InputAction.CallbackContext context)
    {
        if(FlyingItem == null)
        {
            FlyingItem = Instantiate(FlyingItemPrafabs, new Vector3(0, 0, 0), Quaternion.identity);
        }
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="_flyingItem"></param>
    /// <param name="_predictionPoint"></param>
    public void SetFlyingPoints(GameObject _flyingItem,GameObject _endPoint,GameObject _predictionPoint)
    {
        FlyingItem = _flyingItem;
        EndPoint = _endPoint;
        predictionPoint = _predictionPoint;

        lineRendererRef.GetComponent<ParabolaLineRenderer>().GetFlyingPoints(_flyingItem.transform, _endPoint.transform,_predictionPoint.transform);
        lineRendererRef.SetActive(true);
        lineRendererRef.GetComponent<ParabolaLineRenderer>().ActiveLine();
    }

    public void ReSetFlyingPoints()
    {
        FlyingItem = null;
        EndPoint = null;
        predictionPoint = null;
        lineRendererRef.SetActive(false);
    }

    public void UpdateEndPointsPos(Transform endTransform)
    {
        lineRendererRef.GetComponent<ParabolaLineRenderer>().UpdateEndPointPos(endTransform);
    }

    public void UpdatePredictionPointPos(Vector3 prepos) 
    {
        predictionPoint.transform.position = prepos;
        Debug.Log(prepos);
    }

    public int PassConflictSegmentCount()
    {
        return lineRendererRef.GetComponent<ParabolaLineRenderer>().GetConflictSegmentCount();
    }

}
