
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;


    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score : {ScoreManager.Instance.CurrentScore}";
    }
}
