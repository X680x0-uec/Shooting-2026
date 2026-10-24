using System.Collections;
using UnityEngine;

public abstract class SpawnCommonClass : ScriptableObject
{
    // このスクリプトはいじらなくていい(いじる必要があるときは伝えます)
    // 敵の出現処理は全部このクラスを継承
    public abstract IEnumerator Spawn(WeakEnemyInfo date);
}