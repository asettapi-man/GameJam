using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    //シングルトン化
    public static PauseManager Instance { get; private set; }

    [SerializeField, Tooltip("入力方法")]
    private InputAction pauseAction;

    [SerializeField, Tooltip("音量パネル")]
    private GameObject AudioPanel;

    //一時停止中フラグ
    [HideInInspector]
    public bool isPause = false;

    private void Awake()
    {
        //このスクリプトがあるか？
        if (Instance != null)
        {
            //既にあるなら自身を削除
            Destroy(gameObject);
            return;
        }

        Instance = this;

        //キー入力の設定
        pauseAction.Enable();
        pauseAction.performed += OnPause;

        AudioPanel.SetActive(isPause);
    }

    /// <summary>
    /// 入力時に実行
    /// </summary>
    /// <param name="context"></param>
    private void OnPause(InputAction.CallbackContext context)
    {
        //現在のシーン名を取得
        var sceneName = SceneManager.GetActiveScene().name;

        //シーン名が一致・ゲームオーバー時じゃない時か？
        if (sceneName == "GameScene" && !GameManager.Instance.isGameOver)
        {
            PauseSystem();
        }
    }

    /// <summary>
    /// 一時停止と再生を切り替え
    /// </summary>
    public void PauseSystem()
    {
        //フラグを反転
        isPause = !isPause;

        //フラグに合わせて表示・非表示を行う
        if (AudioPanel != null)
        {
            AudioPanel.SetActive(isPause);
        }
    }
}
