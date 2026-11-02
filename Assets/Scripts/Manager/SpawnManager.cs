using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnManager : MonoBehaviour
{
    // 敵の出現を管理するスクリプト
    [SerializeField] private StageInfo[] stageDate; //ステージ1, 2, 3...という風に敵出現データの配列を受け取る
    [SerializeField] private GameManager gameManager;
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
        while (nextSpawnIndex < stageDate[gameManager.stageNumber].spawnDate.Length && currentTime >= stageDate[gameManager.stageNumber].spawnDate[nextSpawnIndex].spawnTime){
            SpawnEnemy(stageDate[gameManager.stageNumber].spawnDate[nextSpawnIndex]);
            nextSpawnIndex++;
        }
        // ザコ敵が時間になったら召喚される
        while (weakNextSpawnIndex < stageDate[gameManager.stageNumber].weakDate.Length && currentTime >= stageDate[gameManager.stageNumber].weakDate[weakNextSpawnIndex].weakSpawnTime){
            StartCoroutine(SpawnWeakEnemy(stageDate[gameManager.stageNumber].weakDate[weakNextSpawnIndex]));
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