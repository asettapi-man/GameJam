using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    //シーン遷移
    public void SwitchSence()
    {
        //書いてある名前の先に遷移する
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
    }

    //シーンの再読み込み
    public void LoadSence()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //ゲーム終了
    public void EndSence()
    {
        //アプリの場合
        Application.Quit();

        //デバックの場合
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
