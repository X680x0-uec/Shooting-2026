using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDate", menuName = "Scriptable Objects/EnemyDate")]
public class EnemyDate : ScriptableObject
{
    // 敵の名前（使わないかも）と体力を設定。これから作られたScriptableObjectはそれぞれの敵へアタッチされたEnemyHPManagerのenemyDateへインスペクターからいれる。
    public string enemyName; // 敵の名前
    public float hp; // 敵の体力
}
