using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [Header("パネルの設定")]
    public GameObject inventoryPanel;   // 開くパネル
    public Button openButton;           // 開くボタン
    public Button closeButton;          // 閉じるボタン（パネル内に配置）

    private bool isOpen = false;        // パネルが開いているか

    void Start()
    {
        // ボタンが押された時に呼ぶ関数を登録
        openButton.onClick.AddListener(TogglePanel);
        closeButton.onClick.AddListener(ClosePanel);

        // 最初はパネルを閉じておく
        inventoryPanel.SetActive(false);
    }

    // 開閉を切り替える
    public void TogglePanel()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);
    }

    // パネルを開く
    public void OpenPanel()
    {
        isOpen = true;
        inventoryPanel.SetActive(true);
    }

    // パネルを閉じる
    public void ClosePanel()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
    }
}