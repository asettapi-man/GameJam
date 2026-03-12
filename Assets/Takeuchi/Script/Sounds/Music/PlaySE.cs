using UnityEngine;

public class PlaySE : MonoBehaviour
{
    // 再生する効果音
    [SerializeField] AudioClip clip;     

    // 音を出すスピーカー
    [SerializeField] AudioSource source;  

    //デバッグ用ブール
    [SerializeField] bool debug;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
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