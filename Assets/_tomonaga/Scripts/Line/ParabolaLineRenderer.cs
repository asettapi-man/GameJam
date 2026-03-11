using UnityEngine;

/// <summary>
/// 2つのオブジェクトの間に放物線を描くクラス
/// LineRendererで軌道を描画し、
/// その軌道上にParticleを生成する
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class ParabolaLineRenderer : MonoBehaviour
{
    // 放物線の開始位置
    private Transform startPoint;

    // 放物線の実際の終了位置
    private Transform endPoint;

    // 放物線の終了位置
    private Transform predictionPoint;

    //衝突した頂点の番号を保存する変数
    private int conflictSegmentCount = 0;

    // LineRendererの分割数（多いほど滑らか）
    [SerializeField,Header("頂点の数（多いほどなめらか）"),Range(10,100)]
    private int segmentCount = 30;

    // 放物線の高さ
    [SerializeField, Header("放物線の高さ"), Range(1f, 5f)]
    private float height = 2f;

    // 軌道上に配置するParticle
    public ParticleSystem particlePrefab;

    // LineRendererコンポーネント
    private LineRenderer line;

    // 生成したParticleを管理する配列
    private ParticleSystem[] particles;

    public float collisionCheckRadius = 0.5f;


    /// <summary>
    /// 初期化処理
    /// </summary>
    void Start()
    {
        // LineRendererを取得
        line = GetComponent<LineRenderer>();

        // LineRendererの頂点数を設定
        line.positionCount = segmentCount;

        // Particle配列を作成
        particles = new ParticleSystem[segmentCount];

        //マネージャーにリファレンスを渡す
        LineManager.Instance.SetParabolaLineRenderer(gameObject);
    }


    /// <summary>
    /// 毎フレーム更新
    /// オブジェクトが動いても放物線が更新される
    /// </summary>
    void Update()
    {
        UpdateParticlePositions();
    }


    /// <summary>
    /// 放物線の始点・終点・予測点を設定
    /// </summary>
    /// <param name="_flyingItem">飛行するアイテム（始点）</param>
    /// <param name="_endPoint">マウスホイールで移動する終点</param>
    /// <param name="_predictionPoint">衝突判定後の実際の着地予測点</param>
    public void GetFlyingPoints(Transform _flyingItem, Transform _endPoint ,Transform _predictionPoint)
    {
        startPoint = _flyingItem;
        endPoint = _endPoint;
        predictionPoint = _predictionPoint;
    }


    /// <summary>
    /// 放物線の描画とパーティクルの配置を実行
    /// </summary>
    public void ActiveLine()
    {
        // 放物線を描画
        DrawParabola();

        // Particleを配置
        SpawnParticles();

    }


    /// <summary>
    /// 放物線を計算してLineRendererに設定する
    /// </summary>
    public void DrawParabola()
    {
        // segmentCount分だけ頂点を計算
        for (int i = 0; i < segmentCount; i++)
        {
            // 0〜1の割合を作る
            float t = i / (float)(segmentCount - 1);

            // startからendまで線形補間
            Vector3 position = Vector3.Lerp(startPoint.position, endPoint.position, t);

            // 放物線の高さを計算
            // t=0.5で最大になる
            float parabola = 4 * height * (t - t * t);

            // Y方向に高さを追加
            position.y += parabola;

            // LineRendererの頂点に設定
            line.SetPosition(i, position);
        }

        // 衝突を検知
        AdjustEndPointPosition();
    }


    /// <summary>
    /// 放物線上にParticleを生成
    /// </summary>
    void SpawnParticles()
    {
        // 既存のパーティクルを全部破棄してからリセット
        ClearParticles();

        for (int i = 2; i < conflictSegmentCount - 2; i++)
        {
            Vector3 pos = line.GetPosition(i);
            ParticleSystem particle = Instantiate(particlePrefab, pos, Quaternion.identity, transform);
            particles[i] = particle;
        }
    }

    /// <summary>
    /// 既存の全パーティクルを破棄
    /// </summary>
    public void ClearParticles()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            if (particles[i] != null)
            {
                Destroy(particles[i].gameObject);
                particles[i] = null;
            }
        }
    }


    /// <summary>
    /// Particleを放物線上に追従させる
    /// </summary>
    void UpdateParticlePositions()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            // Particleが存在する場合
            if (particles[i] != null)
            {
                // LineRendererの頂点位置に移動
                particles[i].transform.position = line.GetPosition(i);
            }
        }
    }

    /// <summary>
    /// LineRendererコンポーネントを取得
    /// </summary>
    /// <returns>LineRendererコンポーネント</returns>
    public LineRenderer GetLineRenderer()
    {
        return line;
    }

    /// <summary>
    /// 終点の衝突判定を行い、衝突している場合は予測点を調整
    /// 放物線の始点側から順に衝突チェックし、最初に衝突した位置の1つ前を予測点とする
    /// </summary>
    void AdjustEndPointPosition()
    {

        // 接触点が存在しない場合は終点が予測地点になる
        if (!Physics2D.OverlapCircle(endPoint.position, collisionCheckRadius))
        {

            conflictSegmentCount = segmentCount;
            LineManager.Instance.UpdatePredictionPointPos(endPoint.position);
            return;
        }



        // 放物線の頂点を始点側からチェック
        for (int i = 0; i < segmentCount-1; i++)
        {

            Vector3 checkConflictPos = line.GetPosition(i);

            // この位置で衝突判定
            if (Physics2D.OverlapCircle(checkConflictPos, collisionCheckRadius))
            {
                // 衝突した位置を発見
                // 衝突位置の1つ前（i-1）を予測点として設定
                LineManager.Instance.UpdatePredictionPointPos(line.GetPosition(Mathf.Max(0, i - 1)));

                conflictSegmentCount = i;

                return;
            }


        }

        // すべての頂点で衝突している場合の処理
        Debug.LogWarning("放物線全体が衝突しています");
    }

    /// <summary>
    /// 終点の位置を更新（マウスホイール操作時に呼ばれる）
    /// </summary>
    /// <param name="transform">新しい終点のTransform</param>
    public void UpdateEndPointPos(Transform transform)
    {
        endPoint.position = transform.position;
    }


    /// <summary>
    /// 衝突が発生した頂点番号を取得
    /// </summary>
    /// <returns>衝突した頂点のインデックス（衝突なしの場合はsegmentCount）</returns>
    public int GetConflictSegmentCount()
    {
        return conflictSegmentCount;
    }
}