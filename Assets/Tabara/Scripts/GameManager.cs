using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //シングルトン化
    public static GameManager Instance { get; private set; }

    [HideInInspector]
    public List<ItemDataBase.ItemData> itemDatas;

    //ゲームオーバーフラグ
    [HideInInspector]
    public bool isGameOver = false;

    private void Awake()
    {
        //自分自身以外がいるか？
        if (Instance != null)
        {
            //自分自身を削除する
            Destroy(gameObject);
            return;
        }

        //自分自身に設定
        Instance = this;

        //シーンをまたいでも消えないようにする
        //DontDestroyOnLoad(gameObject);  
    }
}
