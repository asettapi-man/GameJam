using UnityEngine;

public class SEManeger : MonoBehaviour
{
    [SerializeField] private PlaySE[] playSEs;
    
    //デバッグ用のBOOL
    [SerializeField] private bool debug;

    [SerializeField] private int seNumber;

    void Awake()
    {
        // 子オブジェクトのPlaySEを全て取得
        playSEs = GetComponentsInChildren<PlaySE>();
    }
   public void Play(int seNumber)
    {
        if (seNumber > playSEs.Length) return;
        playSEs[seNumber].PlaySEs();
    }

    private void Update()
    {
        //tureにした時にデバッグとして関数を実行
        if (debug)
        {
            Play(seNumber);
        }
    }
}