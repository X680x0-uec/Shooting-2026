using UnityEngine;

[System.Serializable] public class EnemyInfo{
    // 強めの敵のオブジェクト、出現時間、出現位置を設定。SpawmManagerのインスペクターから設定する
    public GameObject Enemy; // 出現させるオブジェクト
    public float spawnTime; // 出現させる時間
    public Vector2 spawnPosition; // 出現させる位置
}