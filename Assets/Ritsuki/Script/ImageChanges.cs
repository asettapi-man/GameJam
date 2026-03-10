using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;


public class ImageChanges : MonoBehaviour
{
    [SerializeField] private Image displayImage;      // 画像を表示するUIのImageコンポーネント
    [SerializeField] private Sprite[] sprites;        // 切り替える画像をまとめた配列
    [SerializeField] private float interval = 2.0f;   // 切り替え間隔（秒）

    private Coroutine coroutine;

    private void Awake()
    {
        //コルーチン開始
        coroutine = StartCoroutine(AutoSwitch());
    }

    private void Update()
    {
        //左クリックを押したらコルーチンを止める
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("左クリックした！");
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator AutoSwitch()
    {
        //無限ループで画像を切り替える
        while (true)
        {

            //画像をランダムにする
            int index = Random.Range(0, sprites.Length);

            Debug.Log($"乱数：{index}");

            //現在の画像を表示する
            displayImage.sprite = sprites[index];

            //指定した間隔だけ待機する
            yield return new WaitForSeconds(interval);
        }
    }

}
