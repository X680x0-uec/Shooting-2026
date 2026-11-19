using UnityEngine;
using UnityEngine.SceneManagement;

public class ShiftScene : MonoBehaviour
{
    // Inspectorから遷移先のシーン名を自由に指定できるようにする
    [SerializeField] private string targetSceneName = "";

    public void ClickmoveButton()
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("遷移先のシーン名が設定されていません。");
        }
    }
}
