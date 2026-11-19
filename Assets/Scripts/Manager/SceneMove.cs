using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    [SerializeField] private GameObject startImage;
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene_Recovery");
        startImage.SetActive(false);
    }
}
