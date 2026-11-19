using System;
using UnityEngine;

public class EnemyHPManager : MonoBehaviour
{
    // 敵がPlayerの弾に当たったらHPを下げる処理を実行
    [SerializeField] private EnemyDate enemyDate;
    [SerializeField] private float point;
    [SerializeField] private float scale;
    private float hp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hp = enemyDate.hp;
    }

    public void TakeDamege(float power) // 弾に当たったらその弾がこの処理を呼ぶ
    {
        if(hp > 0)
        {
            ScoreManager.Instance.AddScore((int)Mathf.Round(point));
            hp -= power;
        }
        else
        {
            ScoreManager.Instance.DeadScore((int)Mathf.Round(point * scale));
            Destroy(this.gameObject);
        }
    }
}
