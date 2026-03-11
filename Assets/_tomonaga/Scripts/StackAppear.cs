using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StackAppear : MonoBehaviour
{
    [Header("スロット設定")]
    public List<RectTransform> slots = new List<RectTransform>();

    [Header("スプライト設定")]
    public List<Sprite> sprites = new List<Sprite>();

    [Header("拡大アニメーション")]
    public float scaleDuration = 0.25f;
    public float intervalPerSlot = 0.08f;

    [Header("物理崩落設定")]
    public float gravityScale = 3f;
    public float friction = 0.4f;
    public float bounciness = 0.1f;

    private enum State { Idle, Appearing, Done, Falling }
    private State state = State.Idle;

    private List<RectTransform> uiItems = new List<RectTransform>();
    private List<GameObject> physicsItems = new List<GameObject>();

    private PhysicsMaterial2D sharedMat;
    private GameObject floorObj, wallLeft, wallRight;
    private Camera cam;
    private Canvas canvas;

    void Start()
    {

        cam = Camera.main;
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null) canvas = FindFirstObjectByType<Canvas>();

        sharedMat = new PhysicsMaterial2D("StackMat")
        {
            friction = friction,
            bounciness = bounciness
        };

        CreateWalls();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            OnClick();
    }

    void OnClick()
    {
        if (state == State.Idle)
        {
            state = State.Appearing;
            StartCoroutine(AppearRoutine());
        }
        else if (state == State.Done)
        {
            state = State.Falling;
            StartCoroutine(FallRoutine());
        }
    }

    // =============================
    // 出現処理
    // =============================
    IEnumerator AppearRoutine()
    {
        foreach (var item in uiItems)
            if (item != null) Destroy(item.gameObject);
        uiItems.Clear();

        if (sprites.Count == 0)
        {
            Debug.LogError("Sprites リストが空です。");
            state = State.Idle;
            yield break;
        }

        var sorted = slots.OrderBy(s => s.position.y).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            StartCoroutine(
                SpawnAndScale(sorted[i], sprites[Random.Range(0, sprites.Count)]));

            yield return new WaitForSeconds(intervalPerSlot);
        }

        // 全スロットの拡大完了を待つ
        float totalWait = scaleDuration + intervalPerSlot * sorted.Count;
        yield return new WaitForSeconds(totalWait);

        state = State.Done;
        Debug.Log("全スロット出現完了！クリックで崩れます");
    }

    IEnumerator SpawnAndScale(RectTransform slot, Sprite sprite)
    {
        GameObject obj = new GameObject("StackItem");
        obj.transform.SetParent(slot.parent, false);

        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = slot.anchorMin;
        rt.anchorMax = slot.anchorMax;
        rt.anchoredPosition = slot.anchoredPosition;
        rt.sizeDelta = slot.sizeDelta;
        rt.pivot = slot.pivot;
        rt.localScale = Vector3.zero;

        Image img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = false;
        img.raycastTarget = false;

        uiItems.Add(rt);

        float elapsed = 0f;
        while (elapsed < scaleDuration)
        {
            elapsed += Time.deltaTime;
            float s = EaseOutCubic(Mathf.Clamp01(elapsed / scaleDuration));
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }
        rt.localScale = Vector3.one;
    }

    float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);

    // =============================
    // 崩落処理
    // =============================
    IEnumerator FallRoutine()
    {
        foreach (var uiItem in uiItems)
        {
            if (uiItem == null) continue;

            // スクリーン座標 → ワールド座標に変換
            Vector3 screenPos = RectTransformUtility.WorldToScreenPoint(null, uiItem.position);
            Vector3 worldPos;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                uiItem, screenPos, cam, out worldPos);

            // CanvasのRenderModeで変換方法を切り替え
            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                worldPos = cam.ScreenToWorldPoint(
                    new Vector3(screenPos.x, screenPos.y, -cam.transform.position.z));
            }
            else
            {
                worldPos = uiItem.position; // WorldSpaceならそのまま使える
            }

            Vector2 worldSize = GetWorldSize(uiItem);

            GameObject physObj = new GameObject("PhysicsItem");
            physObj.transform.position = new Vector3(worldPos.x, worldPos.y, 0f);

            var sr = physObj.AddComponent<SpriteRenderer>();
            sr.sprite = uiItem.GetComponent<Image>().sprite;
            sr.sortingOrder = 100;

            Vector2 spriteSize = sr.sprite.bounds.size;
            physObj.transform.localScale = new Vector3(
                worldSize.x / spriteSize.x,
                worldSize.y / spriteSize.y,
                1f
            );

            var rb = physObj.AddComponent<Rigidbody2D>();
            rb.gravityScale = gravityScale;
            rb.linearDamping = 0.3f;
            rb.angularDamping = 0.5f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            physicsItems.Add(physObj);
        }

        foreach (var uiItem in uiItems)
            if (uiItem != null) uiItem.gameObject.SetActive(false);

        // 床を消して落下
        floorObj.GetComponent<BoxCollider2D>().enabled = false;

        float bottomY = cam.transform.position.y - cam.orthographicSize - 10f;
        float timeout = 6f, elapsed = 0f;

        while (elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            bool allFallen = physicsItems
                .Where(i => i != null)
                .All(i => i.transform.position.y < bottomY);
            if (allFallen) break;
            yield return null;
        }

        foreach (var item in uiItems)
            if (item != null) Destroy(item.gameObject);
        uiItems.Clear();

        foreach (var item in physicsItems)
            if (item != null) Destroy(item);
        physicsItems.Clear();

        floorObj.GetComponent<BoxCollider2D>().enabled = true;

        state = State.Idle;
        Debug.Log("崩落完了！再度クリックで出現します");
    }

    Vector2 GetWorldSize(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // スクリーン座標をワールド座標に変換してサイズを計算
            Vector3 c0 = cam.ScreenToWorldPoint(new Vector3(corners[0].x, corners[0].y, -cam.transform.position.z));
            Vector3 c1 = cam.ScreenToWorldPoint(new Vector3(corners[1].x, corners[1].y, -cam.transform.position.z));
            Vector3 c3 = cam.ScreenToWorldPoint(new Vector3(corners[3].x, corners[3].y, -cam.transform.position.z));
            return new Vector2(
                Vector3.Distance(c0, c3),
                Vector3.Distance(c0, c1)
            );
        }
        else
        {
            return new Vector2(
                Vector3.Distance(corners[0], corners[3]),
                Vector3.Distance(corners[0], corners[1])
            );
        }
    }

    // =============================
    // 壁・床（Layerなし）
    // =============================
    void CreateWalls()
    {
        float screenH = cam.orthographicSize * 2f;
        float screenW = screenH * cam.aspect;
        float cx = cam.transform.position.x;
        float cy = cam.transform.position.y;
        const float t = 0.2f;

        floorObj = CreateBox("Floor",
            new Vector2(cx, cy - cam.orthographicSize - t / 2f),
            new Vector2(screenW + 1f, t));
        wallLeft = CreateBox("WallL",
            new Vector2(cx - screenW / 2f - t / 2f, cy),
            new Vector2(t, screenH + 4f));
        wallRight = CreateBox("WallR",
            new Vector2(cx + screenW / 2f + t / 2f, cy),
            new Vector2(t, screenH + 4f));
    }

    GameObject CreateBox(string name, Vector2 pos, Vector2 size)
    {
        var go = new GameObject(name);
        go.transform.position = pos;
        var col = go.AddComponent<BoxCollider2D>();
        col.size = size;
        col.sharedMaterial = sharedMat;
        return go;
    }
}