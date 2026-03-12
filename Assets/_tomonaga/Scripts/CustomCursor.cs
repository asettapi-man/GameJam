using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour
{
    [Header("カーソル画像")]
    public Image cursorImage;
    public Sprite defaultSprite;  // 通常時
    public Sprite hoverSprite;    // ホバー時

    [Header("設定")]
    public Vector2 offset = Vector2.zero;

     private float blinkTimer = 0f;
    private bool isImage1 = true;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // カーソル位置を更新
        Vector2 mousePos = Mouse.current.position.ReadValue();
        cursorImage.rectTransform.position = new Vector3(
            mousePos.x + offset.x,
            mousePos.y + offset.y,
            0f
        );

        if (IsHoveringButton())
        {
            // 0.5秒間隔で交互に切り替え
            blinkTimer += Time.deltaTime;
            if (blinkTimer >= 0.5f)
            {
                blinkTimer = 0f;
                isImage1 = !isImage1;
            }
            cursorImage.sprite = isImage1 ? defaultSprite : hoverSprite;
        }
        else
        {
            // ホバー外れたらタイマーリセットして画像1に戻す
            blinkTimer = 0f;
            isImage1 = true;
            cursorImage.sprite = defaultSprite;
        }
    }

    bool IsHoveringButton()
    {
        var eventSystem = EventSystem.current;
        if (eventSystem == null) return false;

        // マウス位置にあるUIを取得
        var pointerData = new PointerEventData(eventSystem)
        {
            position = Mouse.current.position.ReadValue()
        };

        var results = new System.Collections.Generic.List<RaycastResult>();
        eventSystem.RaycastAll(pointerData, results);

        // Button or 任意のUI要素があればtrue
        foreach (var result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null)
                return true;
        }
        return false;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        Cursor.visible = !hasFocus;
        cursorImage.gameObject.SetActive(hasFocus);
    }
}