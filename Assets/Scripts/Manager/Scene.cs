using UnityEngine;
using UnityEngine.SceneManagement;
public class Scene : MonoBehaviour {
    //Sceneを変更するときに参照するオブジェクトです
    public void SceneChange(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
