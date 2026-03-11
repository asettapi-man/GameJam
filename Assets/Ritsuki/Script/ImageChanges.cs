using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;


public class ImageChanges : MonoBehaviour
{
    [SerializeField] private SpriteRenderer displayImage;      // 画像を表示用
    [SerializeField] private ItemDataBase itemDatas;           //アイテムデータ（画像用）
    [SerializeField] private float interval = 2.0f;            // 切り替え間隔（秒）

    //アイテムデータ1件分
    [HideInInspector]
    public ItemDataBase.ItemData currentData;

    private Coroutine coroutine;    //コルーチン

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
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator AutoSwitch()
    {
        //無限ループで画像を切り替える
        while (true)
        {

            //画像をランダムにする
            int index = Random.Range(0, itemDatas.datas.Count);

            //現在の画像を表示する
            currentData = itemDatas.datas[index];
            displayImage.sprite = currentData.sprite;
            var scale = currentData.scale;
            displayImage.gameObject.GetComponent<Transform>().localScale = new Vector3(scale, scale, scale);

            //指定した間隔だけ待機する
            yield return new WaitForSeconds(interval);
        }
    }
}
