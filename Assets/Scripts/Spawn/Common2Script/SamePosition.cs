using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "SamePosition", menuName = "Scriptable Objects/MovePettern/SamePosition")]
public class SamePosition : SpawnCommonClass
{
    public override IEnumerator Spawn(WeakEnemyInfo date)
    {
        TimeManager timeManager = GameObject.FindAnyObjectByType<TimeManager>();
        int i = 0;
        while(i < date.weakSpawnNumber)
        {
            if (!timeManager.IstimePassing){
                yield return new WaitForSeconds(0);
            }
            else{
                GameObject enemy = Object.Instantiate(date.weakEnemy, date.weakSpawnPosition, Quaternion.identity); // 同じ位置に敵を召喚
                i++;
                yield return new WaitForSeconds(date.interval);
            }
        }
    }
}