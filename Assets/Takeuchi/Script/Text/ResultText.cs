using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトUIの数字データ用コンポーネント
/// 仮の数値を設定してTextに表示する
/// </summary>
public class ResultText : MonoBehaviour
{
    [SerializeField]
    private Text text; // 表示するUIテキスト

    void Awake()
    {
        // Textが未設定なら自動取得
        if (text == null)
            text = GetComponent<Text>();
    }
    public void UpdateText(int counts)
    {
        text.text = counts.ToString();
    }
}