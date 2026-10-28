using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Scriptable Objects/StageInfo")]
public class StageInfo : ScriptableObject
{
    public EnemyInfo[] spawnDate; // 微～強敵の出現情報の配列
    public WeakEnemyInfo[] weakDate; // ザコ敵の出現情報の配列
}
