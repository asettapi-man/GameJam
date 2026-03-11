using UnityEngine;
//アタッチされたimageゲームオブジェクトのアクティブ非アクティブにを切り替える
public class SwitchingActiveImage : MonoBehaviour
{
    // このオブジェクトを表示
    public void Display()
    {
        gameObject.SetActive(true);
    }

    // このオブジェクトを非表示
    public void Hidden()
    {
        gameObject.SetActive(false);
    }
}