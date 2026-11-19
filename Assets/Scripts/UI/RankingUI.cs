using UnityEngine;
using TMPro;

public class RankingUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text[] rankingTexts;

    private void Start()
    {
        for (int i = 0; i < rankingTexts.Length; i++)
        {
            int score = ScoreManager.Instance.GetRankingScore(i);

            rankingTexts[i].text = $"{i + 1}位：{score}";
        }
    }
}