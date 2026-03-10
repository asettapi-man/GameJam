using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlyingItem : MonoBehaviour
{
    [SerializeField,Header("予測地点用オブジェクト")] 
    private GameObject predictionPointPrafabs;

    [SerializeField, Header("終点用オブジェクト")]
    private GameObject endPointPrafabs;

    [SerializeField, Header("予測地点用オブジェクト色")]
    private Color predictionPointColor;

    [SerializeField, Header("(仮置き)アイテム画像")]
    private Sprite itemSprite;

    [SerializeField, Header("予測地点までの距離"), Range(0, 100)]
    private int predictionDistance;

    private GameObject endObj;

    private GameObject preObj;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 生成したオブジェクトを変数に入れて操作する
        endObj = Instantiate(endPointPrafabs, new Vector3(predictionDistance, 0, 0), Quaternion.identity);
        preObj = Instantiate(predictionPointPrafabs, new Vector3(predictionDistance, 0, 0), Quaternion.identity);
        SpriteRenderer spriteRenderer = preObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = itemSprite;
        spriteRenderer.color = predictionPointColor;
        Debug.Log("予測地点生成");

        LineManager.Instance.SetFlyingPoints(this.gameObject, endObj,preObj);
    }

    public void FinishFlying()
    {
        Destroy(GetComponent<MoveAlongLine>());
        gameObject.AddComponent<Rigidbody2D>();
        gameObject.AddComponent<PolygonCollider2D>();
        Destroy(GetComponent<FlyingItem>());

        LineManager.Instance.ReSetFlyingPoints();
        Destroy(endObj);
        Destroy(preObj);
    }

}
