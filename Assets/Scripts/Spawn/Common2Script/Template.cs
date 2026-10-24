using System.Collections;
using UnityEngine;
// fileName="${ファイルの名前}", munuName = "Objects/${何かの名前}/${ファイルの名前}"　みたいにする
[CreateAssetMenu(fileName = "Template", menuName = "Scriptable Objects/MovePettern/Template")]
//public class ${ファイルの名前} : SpawnCommonClass　っていう風に変える
public class Template : SpawnCommonClass
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
                /*
                    forとyield returnの間のここに出現処理を書いていく。
                    これで多分大体の連続して出現する処理は書けると思いますが、不便なところがあれば教えてください
                */
                i++;
                yield return new WaitForSeconds(date.interval); // date.intervalの間隔で
            }
        }
    }
}