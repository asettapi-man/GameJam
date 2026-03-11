using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 合計金額をテキストに表示するスクリプト
/// </summary>
public class ResultTotalText : MonoBehaviour
{
    // 計算スクリプト
    [SerializeField] private AccountingCalculations accounting;
    // 表示テキスト
    [SerializeField] private Text totalText;
    // 何秒後に表示するか　計算が完了する前にテキストの更新をしないようにする保険
    [SerializeField] private float delayTime = 0.5f; 

    void Start()
    {
        totalText = GetComponent<Text>();

        // 指定秒後にテキスト更新
        StartCoroutine(UpdateTextDelay());
    }

    /// <summary>
    /// 指定秒後にテキストを更新
    /// </summary>
    IEnumerator UpdateTextDelay()
    {
        yield return new WaitForSeconds(delayTime);

        UpdateText();
    }

    /// <summary>
    /// テキスト更新
    /// </summary>
    public void UpdateText()
    {
        if (accounting != null && totalText != null)
        {
            totalText.text = accounting.totalAmount + "円";
        }
    }
}