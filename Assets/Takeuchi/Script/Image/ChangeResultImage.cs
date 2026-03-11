using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトUIの画像表示用
/// </summary>
public class ChangeResultImage : MonoBehaviour
{
    [SerializeField] private Image image;

    private void Awake()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    /// <summary>
    /// 画像設定
    /// </summary>
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}