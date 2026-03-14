using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StackAppear : MonoBehaviour
{
    [Header("グリッド設定")]
    public int rows = 3;
    public int columns = 4;
    public Vector2 slotSize = new Vector2(100f, 100f);
    public Vector2 spacing = new Vector2(10f, 10f);
    public Vector2 gridAnchor = new Vector2(0.5f, 0.5f);

    [Header("スプライト設定")]
    public List<Sprite> sprites = new List<Sprite>();

    [Header("アニメーション設定")]
    public float scaleDuration = 0.25f;
    public float intervalPerSlot = 0.08f;

    private enum State { Idle, Appearing, Disappearing }
    private State state = State.Idle;

    private List<RectTransform> slots = new List<RectTransform>();
    private List<RectTransform> uiItems = new List<RectTransform>();

    private Canvas canvas;
    private RectTransform canvasRect;
    private GameObject slotContainer;
    private EventSystem eventSystem;

    void Start()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null) canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        eventSystem = EventSystem.current;
        GenerateSlots();
    }

    // =============================
    // 外部から呼ぶ公開メソッド
    // =============================

    /// <summary>
    /// ボタンのOnClickに登録する。出現→シーン遷移→消滅を一連で実行。
    /// </summary>
    public void StartAppear(string nextSceneName)
    {
        if (state != State.Idle) return;
        state = State.Appearing;
        StartCoroutine(AppearThenTransition(nextSceneName));
    }

    // =============================
    // 出現 → 遷移
    // =============================
    IEnumerator AppearThenTransition(string nextSceneName)
    {
        foreach (var item in uiItems)
            if (item != null) Destroy(item.gameObject);
        uiItems.Clear();

        if (sprites.Count == 0)
        {
            Debug.LogError("Sprites リストが空です。");
            state = State.Idle;
            //SetInput(true);
            yield break;
        }

        var sorted = slots.OrderBy(s => s.position.y).ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            StartCoroutine(SpawnAndScale(sorted[i], sprites[Random.Range(0, sprites.Count)]));
            yield return new WaitForSeconds(intervalPerSlot);
        }

        // 全スロットの拡大完了を待つ
        yield return new WaitForSeconds(scaleDuration + intervalPerSlot * sorted.Count);

        // Canvas・EventSystemをDontDestroyOnLoadに登録して遷移後も保持
        DontDestroyOnLoad(canvas.gameObject);

        // シーン遷移（非同期で待機）
        var op = SceneManager.LoadSceneAsync(nextSceneName);
        yield return op;

        eventSystem = EventSystem.current;

        // 遷移完了後、即座に消滅開始
        state = State.Disappearing;
        StartCoroutine(DisappearRoutine());
    }

    // =============================
    // 消滅（上から縮小）
    // =============================
    IEnumerator DisappearRoutine()
    {
        var sorted = uiItems
            .Where(i => i != null)
            .OrderByDescending(i => i.position.y)
            .ToList();

        for (int i = 0; i < sorted.Count; i++)
        {
            StartCoroutine(ScaleAndDestroy(sorted[i]));
            yield return new WaitForSeconds(intervalPerSlot);
        }

        yield return new WaitForSeconds(scaleDuration + intervalPerSlot * sorted.Count);

        foreach (var item in uiItems)
            if (item != null) Destroy(item.gameObject);
        uiItems.Clear();

        // 消滅完了後に入力を解放
        state = State.Idle;

        // Canvas自体も不要なら破棄
        Destroy(canvas.gameObject);
    }

    IEnumerator ScaleAndDestroy(RectTransform rt)
    {
        yield return ScaleTo(rt, 1f, 0f, EaseInCubic);
        if (rt != null) Destroy(rt.gameObject);
    }

    // =============================
    // 共通スケールアニメ
    // =============================
    IEnumerator SpawnAndScale(RectTransform slot, Sprite sprite)
    {
        var obj = new GameObject("StackItem");
        obj.transform.SetParent(slot.parent, false);

        var rt = obj.AddComponent<RectTransform>();
        rt.anchorMin = slot.anchorMin;
        rt.anchorMax = slot.anchorMax;
        rt.anchoredPosition = slot.anchoredPosition;
        rt.sizeDelta = slot.sizeDelta;
        rt.pivot = slot.pivot;
        rt.localScale = Vector3.zero;

        var img = obj.AddComponent<Image>();
        img.sprite = sprite;
        img.preserveAspect = false;
        img.raycastTarget = false;

        uiItems.Add(rt);

        yield return ScaleTo(rt, 0f, 1f, EaseOutCubic);
    }

    IEnumerator ScaleTo(RectTransform rt, float from, float to,
                        System.Func<float, float> ease)
    {
        float elapsed = 0f;
        while (elapsed < scaleDuration)
        {
            if (rt == null) yield break;
            elapsed += Time.deltaTime;
            float s = Mathf.Lerp(from, to, ease(Mathf.Clamp01(elapsed / scaleDuration)));
            rt.localScale = new Vector3(s, s, 1f);
            yield return null;
        }
        if (rt != null) rt.localScale = new Vector3(to, to, 1f);
    }

    float EaseOutCubic(float t) => 1f - Mathf.Pow(1f - t, 3f);
    float EaseInCubic(float t) => t * t * t;

    // =============================
    // スロット生成
    // =============================
    void GenerateSlots()
    {
        if (slotContainer != null) Destroy(slotContainer);
        slots.Clear();

        slotContainer = new GameObject("SlotContainer");
        var containerRect = slotContainer.AddComponent<RectTransform>();
        containerRect.SetParent(canvasRect, false);

        float totalW = columns * slotSize.x + (columns - 1) * spacing.x;
        float totalH = rows * slotSize.y + (rows - 1) * spacing.y;

        containerRect.anchorMin = gridAnchor;
        containerRect.anchorMax = gridAnchor;
        containerRect.pivot = new Vector2(0.5f, 0.5f);
        containerRect.sizeDelta = new Vector2(totalW, totalH);
        containerRect.anchoredPosition = Vector2.zero;

        for (int r = 0; r < rows; r++)
            for (int c = 0; c < columns; c++)
            {
                var go = new GameObject($"Slot_{r}_{c}");
                var rt = go.AddComponent<RectTransform>();
                rt.SetParent(containerRect, false);
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.zero;
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.sizeDelta = slotSize;
                rt.anchoredPosition = new Vector2(
                    c * (slotSize.x + spacing.x) + slotSize.x * 0.5f,
                    r * (slotSize.y + spacing.y) + slotSize.y * 0.5f);
                slots.Add(rt);
            }
    }
}