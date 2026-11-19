using System.Collections;
using UnityEngine;

public class Fan2Player : MonoBehaviour
{
    private PlayerController player;
    private TimeManager timeManager;
    private bool isShoot = false;
    private bool isFinished = false;
    private float currentTime;
    private Coroutine shootCoroutine;

    [Header("プレハブ設定")]
    [SerializeField] private GameObject enemyBullet; // Fan.csが付いた弾のプレハブ

    [Header("弾の性能・扇状設定")]
    [SerializeField] private int bulletCount = 5;     // 一度に撃つ弾数（1発〜何発でも可）
    [SerializeField] private float fanAngle = 60f;    // 全体の拡散角度（例: 60度）
    [SerializeField] private float bulletSpeed = 7f;  // 弾速

    [Header("発射タイミング・連射設定")]
    [SerializeField] private float interval = 0.6f;   // 連射速度（発射間隔・秒）
    [SerializeField] private float startTime = 1.0f;  // 出現から発射開始までの時間（秒）
    [SerializeField] private float endTime = 7.0f;    // 出現から発射終了までの時間（秒）

    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>();
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
    }

    void Update()
    {
        if (timeManager != null && timeManager.IstimePassing)
        {
            currentTime += Time.deltaTime;

            // 発射開始タイミング
            if (currentTime >= startTime && !isShoot && !isFinished)
            {
                shootCoroutine = StartCoroutine(ShootLoop());
                isShoot = true;
            }

            // 発射終了タイミング
            if (currentTime >= endTime && isShoot && !isFinished)
            {
                if (shootCoroutine != null)
                {
                    StopCoroutine(shootCoroutine);
                }
                isFinished = true;
            }
        }
    }

    // 連射コルーチン
    private IEnumerator ShootLoop()
    {
        while (true)
        {
            if (timeManager.IstimePassing)
            {
                ShootFan();

                // ポーズ時間に対応した待機処理
                float waitTimer = 0f;
                while (waitTimer < interval)
                {
                    if (timeManager.IstimePassing)
                    {
                        waitTimer += Time.deltaTime;
                    }
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    // 自機方向を基準に扇状へ弾を発射する処理
    private void ShootFan()
    {
        if (enemyBullet == null) return;

        // プレイヤーへの方向ベクトルを取得（プレイヤー不在時は真下）
        Vector2 toPlayer = Vector2.down;
        if (player != null)
        {
            toPlayer = (player.transform.position - transform.position).normalized;
        }

        // 自機方向の基準角度（度）を計算
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        // 1発のみの場合は自機へ向けて真っ直ぐ発射
        if (bulletCount <= 1)
        {
            CreateBullet(baseAngle);
            return;
        }

        // 弾同士の角度間隔と開始角度を計算
        float angleStep = fanAngle / (bulletCount - 1);
        float startAngle = baseAngle - (fanAngle / 2f);

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            CreateBullet(currentAngle);
        }
    }

    // 角度から方向ベクトルを作って弾を生成・設定する
    private void CreateBullet(float angleDegree)
    {
        // 角度(度)をラジアンに変換し、方向ベクトル(X, Y)を算出
        float rad = angleDegree * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);

        // 弾の向き（見た目）も進行方向に合わせる場合（画像が上向き基準なら -90f）
        Quaternion rot = Quaternion.Euler(0f, 0f, angleDegree - 90f);

        GameObject bulletObj = Instantiate(enemyBullet, transform.position, rot);

        // 生成した弾の Fan スクリプトに方向と弾速をセットする
        Fan fanScript = bulletObj.GetComponent<Fan>();
        if (fanScript != null)
        {
            fanScript.direction = dir;
            fanScript.bulletSpeed = bulletSpeed;
        }
    }
}