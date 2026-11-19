using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }

    private const int RankingCount = 5; // この値は、上位何位まで掲示したいかによって値を変える。(これだと上位5位までのスコアを記憶する)
    private const string RankingKey = "Ranking_";

    private bool hasSavedScore = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int point)
    {
        CurrentScore += point;
        Debug.Log(CurrentScore);
    }

    // タイトルからゲームシーンに移動するとき、ScoreManager.Instance.ResetScore(); を実行する。
    public void ResetScore()
    {
        CurrentScore = 0;
        hasSavedScore = false;
    }

    // ゲームシーンからゲームオーバー画面、クリア画面、タイトルどれかに行くとき、ScoreManager.Instance.SaveScoreToRanking(); を実行する。
    public void SaveScoreToRanking()
    {
        if (hasSavedScore) return;
        hasSavedScore = true;

        for (int i = 0; i < RankingCount; i++)
        {
            int oldScore = PlayerPrefs.GetInt(RankingKey + i, 0);

            if (CurrentScore > oldScore)
            {
                for (int j = RankingCount - 1; j > i; j--)
                {
                    int previousScore =
                        PlayerPrefs.GetInt(RankingKey + (j - 1), 0);

                    PlayerPrefs.SetInt(RankingKey + j, previousScore);
                }

                PlayerPrefs.SetInt(RankingKey + i, CurrentScore);
                PlayerPrefs.Save();

                break;
            }
        }
    }

    public int GetRankingScore(int rank)
    {
        if (rank < 0 || rank >= RankingCount) return 0;

        return PlayerPrefs.GetInt(RankingKey + rank, 0);
    }

    public void DeadScore(int point)
    {
        CurrentScore += point;
        Debug.Log(CurrentScore);
    }
}
