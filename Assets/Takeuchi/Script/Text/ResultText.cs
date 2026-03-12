using UnityEngine;
using UnityEngine.UI;
using static ItemDataBase;
/// <summary>
/// リザルトUIの数字表示用
/// </summary>
public class ResultText : MonoBehaviour
{
    [SerializeField] private Text text;

    void Awake()
    {
        if (text == null)
            text = GetComponent<Text>();
    }

    /// <summary>
    /// 数字表示
    /// </summary>
    public void SetText(int value)
    {
        text.text = value.ToString();
    }
}