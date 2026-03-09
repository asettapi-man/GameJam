using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //シーン遷移
    public void SwitchSence()
    {
        //書いてある名前の先に遷移する
        SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
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
