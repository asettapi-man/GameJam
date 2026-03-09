using Unity.VisualScripting;
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
    public Transform startPoint;

    // 放物線の終了位置
    public Transform endPoint;

    // LineRendererの分割数（多いほど滑らか）
    public int segmentCount = 30;

    // 放物線の高さ
    public float height = 2f;

    // 軌道上に配置するParticle
    public ParticleSystem particlePrefab;

    // LineRendererコンポーネント
    private LineRenderer line;

    // 生成したParticleを管理する配列
    private ParticleSystem[] particles;


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

        // 放物線を描画
        DrawParabola();

        // Particleを配置
        SpawnParticles();

        //マネージャーにリファレンスを渡す
        LineManager.Instance.SetParabolaLineRenderer(this);
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
    }


    /// <summary>
    /// 放物線上にParticleを生成
    /// </summary>
    void SpawnParticles()
    {
        for (int i = 0; i < segmentCount; i++)
        {
            // LineRendererの頂点位置を取得
            Vector3 pos = line.GetPosition(i);

            // Particleを生成
            ParticleSystem particle = Instantiate(particlePrefab, pos, Quaternion.identity, transform);

            // 配列に保存
            particles[i] = particle;
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
}