using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ImageChanges : MonoBehaviour
{
    public Image displayImage;      // 画像を表示するUIのImageコンポーネント
    public Sprite[] sprites;        // 切り替える画像をまとめた配列
    public float interval = 2.0f;   // 切り替え間隔（秒）

    void Start()
    {
        //処理を途中で一時停止してから、再開できる
        StartCoroutine(AutoSwitch());
    }

    IEnumerator AutoSwitch()
    {
        //無限ループで画像を切り替える
        while (true)
        {

            //画像をランダムにする
            int index = Random.Range(0, sprites.Length);

            //現在の画像を表示する
            displayImage.sprite = sprites[index];

            //指定した間隔だけ待機する
            yield return new WaitForSeconds(interval);
        }
    }

}
