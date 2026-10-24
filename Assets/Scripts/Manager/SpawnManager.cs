using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    // 敵の出現を管理するスクリプト
    // ステージによって出現をどう変えるかは未実装なので、どこかは変える必要がありそうです。
    [SerializeField] private EnemyInfo[] spawnData; // 微～強敵の出現情報の配列
    [SerializeField] private WeakEnemyInfo[] weakDate; // ザコ敵の出現情報の配列
    [SerializeField] private TimeManager timeManager;
    private float currentTime;
    private int nextSpawnIndex = 0; // 微～強敵の配列で次に出現する情報が入っているインデックス
    private int weakNextSpawnIndex = 0; // ザコ敵の配列で次に出現する情報が入っているインデックス
    private bool start = false; // テストプレイ用
    void Update()
    {
        //このstartはテストプレイ用で、Qキーを押したら開始っていうやつなので、実際出すときになったら消しましょう
        if(Keyboard.current.qKey.wasPressedThisFrame) start = true;
        if(start && timeManager.IstimePassing) currentTime += Time.deltaTime;
        // 微～強敵が時間になったら召喚される
        while (nextSpawnIndex < spawnData.Length && currentTime >= spawnData[nextSpawnIndex].spawnTime){
            SpawnEnemy(spawnData[nextSpawnIndex]);
            nextSpawnIndex++;
        }
        // ザコ敵が時間になったら召喚される
        while (weakNextSpawnIndex < weakDate.Length && currentTime >= weakDate[weakNextSpawnIndex].weakSpawnTime){
            StartCoroutine(SpawnWeakEnemy(weakDate[weakNextSpawnIndex]));
            weakNextSpawnIndex++;
        }
    }

    private void SpawnEnemy(EnemyInfo data)
    {
        Instantiate(data.Enemy, data.spawnPosition, Quaternion.identity);
    }

    private IEnumerator SpawnWeakEnemy(WeakEnemyInfo date){
        // 敵の出現方法はいろいろあると思ったので、enumとか使うにしろ条件分岐が増えそうだからWeakEnemyInfoに設定できるようにしました
        yield return StartCoroutine(date.spawnFunc.Spawn(date));
    }
}