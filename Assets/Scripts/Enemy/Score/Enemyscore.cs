using UnityEngine;

public class Enemyscore : MonoBehaviour
{
    [Header("スコア設定")]
    [SerializeField] private int hitScore = 10;    // 射撃が当たった時のスコア
    [SerializeField] private int defeatScore = 100; // 撃破した時のスコア

    private bool isDead = false; // 撃破スコアの二重加算防止用

    // 射撃が当たった時に、既存のダメージ処理側から呼び出す
    public void AddHitScore()
    {
        if (isDead) return;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(hitScore);
        }
    }

    // 撃破された時に、既存の死亡処理側から呼び出す
    public void AddDefeatScore()
    {
        if (isDead) return;
        isDead = true;

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.DeadScore(defeatScore);
        }
    }
}