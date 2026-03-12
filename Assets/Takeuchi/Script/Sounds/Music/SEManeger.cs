using UnityEngine;

public class SEManeger : MonoBehaviour
{
    public static SEManeger Instance;

    [SerializeField] private PlaySE[] playSEs;

    //デバッグ用
    [SerializeField] private bool debug;
    [SerializeField] private int seNumber;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;

            // シーンを跨いでも破棄しない
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 子オブジェクトのPlaySEを取得
        playSEs = GetComponentsInChildren<PlaySE>();
    }

    public void Play(int seNumber)
    {
        if (seNumber >= playSEs.Length) return;

        playSEs[seNumber].PlaySEs();
    }

    void Update()
    {
        // デバッグ用
        if (debug)
        {
            Play(seNumber);
            debug = false; // 1回だけ再生
        }
    }
}