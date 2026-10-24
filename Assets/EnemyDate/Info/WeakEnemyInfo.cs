using System;
using UnityEngine;

[System.Serializable] public class WeakEnemyInfo
{
    // 弱い敵のオブジェクト、出現時間、出現位置、出現する数、その間隔、出現方法を設定。SpawnManagerのインスペクターから設定する。
    public GameObject weakEnemy; // 出現させる敵
    public float weakSpawnTime; // 出現させる時間
    public Vector2 weakSpawnPosition; // 出現させる位置
    public int weakSpawnNumber; // 出現させる数
    public float interval; // 出現させる間隔
    public SpawnCommonClass spawnFunc; // 出現方法
}