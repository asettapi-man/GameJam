using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneManagement : MonoBehaviour
{
    public float waitTime;

    //シーン遷移
    public void SwitchSence(string sceneName)
    {
        // シーンが存在するか確認
        if (!SceneExists(sceneName))
        {
            Debug.LogWarning($"シーン「{sceneName}」が存在しません");
            return;
        }
       
        //waitTimeの時間待ってからシーン遷移する
        StartCoroutine(SceneDelay(sceneName, waitTime));
    }

     IEnumerator SceneDelay(string sceneName, float waitTime)
    {
        yield return new WaitForSeconds(waitTime); // 1秒待つ

        // シーンが存在したらロード
        SceneManager.LoadScene(sceneName);
    }

    bool SceneExists(string sceneName)
    {
        // Build Settingsに登録されている全シーンを確認
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            //Build Settingsのi番からシーンのファイルパスを取得
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            //パスからファイル名だけを取り出して、拡張子を除いた名前
            string name = System.IO.Path.GetFileNameWithoutExtension(path);


            if (name == sceneName)
                return true; // シーンが見つかった
        }
        return false; // シーンが見つからなかった
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
