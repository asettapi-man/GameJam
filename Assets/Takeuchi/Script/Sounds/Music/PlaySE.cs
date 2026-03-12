using UnityEngine;
/// <summary>
///　効果音のaudioソースコンポーネント
/// </summary>
public class PlaySE : MonoBehaviour
{
    /// <summary>
    /// インスペクターで指定
    /// 再生する効果音
    /// </summary>
    [SerializeField] AudioClip clip;     

    // 音を出すスピーカー
    [SerializeField] AudioSource source;  

    //デバッグ用ブール
    [SerializeField] bool debug;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    /// <summary>
    /// 
    /// </summary>
    public void PlaySEs()
    {
        if (clip == null)
        {
            Debug.LogWarning("clipが設定されていません");
            return;
        }

        source.PlayOneShot(clip);
    }

    private void Update()
    {
        if (debug)
        {
            //デバッグ用
            PlaySEs();
        }
       
    }
}