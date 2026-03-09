using UnityEngine;
using UnityEngine.InputSystem;

public class FlyingItem : MonoBehaviour
{
    [SerializeField,Header("予測地点用オブジェクト")] 
    private GameObject predictionPoint;

    [SerializeField, Header("予測地点用オブジェクト色")]
    private Color predictionPointColor;

    [SerializeField, Header("(仮置き)アイテム画像")]
    private Sprite itemSprite;

    [SerializeField, Header("予測地点までの距離"), Range(0, 100)]
    private int predictionDistance;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 生成したオブジェクトを変数に入れて操作する
        GameObject obj = Instantiate(predictionPoint, new Vector3(predictionDistance, 0, 0), Quaternion.identity);
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSprite;
        spriteRenderer.color = predictionPointColor;
        Debug.Log("予測地点生成");

        LineManager.Instance.SetFlyingPoints(this.gameObject, obj);
    }

}
