using System.Collections;
using UnityEngine;

public class BossDualAttack : MonoBehaviour
{
    private PlayerController player;
    private TimeManager timeManager;
    private bool isAttacking = false;

    private Coroutine fanCoroutine;
    private Coroutine aimCoroutine;

    // プレイヤーの速度算出用（Rigidbody2Dがなくても自動計算できるようにする）
    private Vector3 lastPlayerPos;
    private Vector3 playerVelocity;

    [Header("共通タイミング設定")]
    [SerializeField] private float startTime = 1.5f;      // 出現から攻撃開始までの時間（秒）
    [SerializeField] private bool isInfinite = true;      // 倒されるまでずっと撃ち続けるか
    [SerializeField] private float endTime = 999f;        // isInfiniteがfalseの場合の終了時間

    [Header("攻撃①：扇状弾幕（バラマキ・牽制用）")]
    [SerializeField] private GameObject fanBulletPrefab;  // Fan.csが付いた通常弾プレハブ
    [SerializeField] private int fanBulletCount = 7;      // 一度に撃つ弾数
    [SerializeField] private float fanAngle = 90f;        // 全体の拡散角度
    [SerializeField] private float fanBulletSpeed = 6f;   // 扇状弾の弾速
    [SerializeField] private float fanInterval = 0.8f;    // 扇状弾の発射間隔（秒）

    [Header("攻撃②：偏差撃ち低速大型弾（移動制限・圧迫用）")]
    [SerializeField] private GameObject bigBulletPrefab;  // Fan.csが付いた大型弾プレハブ
    [SerializeField] private float bigBulletSpeed = 3.5f; // 大型弾の弾速（低速）
    [SerializeField] private float bigInterval = 2.2f;    // 大型弾の発射間隔（秒）
    [Range(0f, 1.5f)]
    [SerializeField] private float leadAccuracy = 1.0f;   // 偏差の効き具合（1.0で完全予測、0で通常の自機狙い）

    private float currentTime = 0f;

    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>();
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();

        if (player != null)
        {
            lastPlayerPos = player.transform.position;
        }
    }

    void Update()
    {
        if (timeManager == null || !timeManager.IstimePassing) return;

        currentTime += Time.deltaTime;

        // プレイヤーの現在速度を毎フレーム計測（偏差撃ち計算に使用）
        if (player != null && Time.deltaTime > 0f)
        {
            playerVelocity = (player.transform.position - lastPlayerPos) / Time.deltaTime;
            lastPlayerPos = player.transform.position;
        }

        // 攻撃開始
        if (currentTime >= startTime && !isAttacking)
        {
            isAttacking = true;
            fanCoroutine = StartCoroutine(FanShootLoop());
            aimCoroutine = StartCoroutine(PredictiveBigShootLoop());
        }

        // 攻撃終了（時間制限を使う場合）
        if (!isInfinite && currentTime >= endTime && isAttacking)
        {
            StopAllAttacks();
        }
    }

    // 攻撃停止用（ボス撃破時などに外部から呼ぶことも可能）
    public void StopAllAttacks()
    {
        if (fanCoroutine != null) StopCoroutine(fanCoroutine);
        if (aimCoroutine != null) StopCoroutine(aimCoroutine);
        isAttacking = false;
    }

    // ==========================================
    // ① 扇状弾幕のループ
    // ==========================================
    private IEnumerator FanShootLoop()
    {
        while (true)
        {
            if (timeManager.IstimePassing)
            {
                ShootFan();
                yield return WaitWithPause(fanInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ShootFan()
    {
        if (fanBulletPrefab == null) return;

        Vector2 toPlayer = Vector2.down;
        if (player != null)
        {
            toPlayer = (player.transform.position - transform.position).normalized;
        }

        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        if (fanBulletCount <= 1)
        {
            SpawnBullet(fanBulletPrefab, baseAngle, fanBulletSpeed);
            return;
        }

        float angleStep = fanAngle / (fanBulletCount - 1);
        float startAngle = baseAngle - (fanAngle / 2f);

        for (int i = 0; i < fanBulletCount; i++)
        {
            SpawnBullet(fanBulletPrefab, startAngle + (angleStep * i), fanBulletSpeed);
        }
    }

    // ==========================================
    // ② 偏差撃ち低速大型弾のループ
    // ==========================================
    private IEnumerator PredictiveBigShootLoop()
    {
        // 扇状弾と発射タイミングを少しずらす場合はここで少し待つ
        yield return WaitWithPause(0.4f);

        while (true)
        {
            if (timeManager.IstimePassing)
            {
                ShootPredictiveBigBullet();
                yield return WaitWithPause(bigInterval);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void ShootPredictiveBigBullet()
    {
        if (bigBulletPrefab == null) return;

        Vector3 targetPos = transform.position + Vector3.down;

        if (player != null)
        {
            Vector3 playerPos = player.transform.position;
            float distance = Vector3.Distance(transform.position, playerPos);

            // 弾がプレイヤーの距離に届くまでの予測時間
            float timeToReach = (bigBulletSpeed > 0f) ? (distance / bigBulletSpeed) : 0f;

            // 未来予測位置 = 現在位置 + (プレイヤー速度 × 予測時間 × 補正係数)
            targetPos = playerPos + (playerVelocity * timeToReach * leadAccuracy);
        }

        Vector2 shootDir = (targetPos - transform.position).normalized;
        float aimAngle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;

        SpawnBullet(bigBulletPrefab, aimAngle, bigBulletSpeed);
    }

    // ==========================================
    // 共通の弾生成ヘルパー
    // ==========================================
    private void SpawnBullet(GameObject prefab, float angleDegree, float speed)
    {
        float rad = angleDegree * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        Quaternion rot = Quaternion.Euler(0f, 0f, angleDegree - 90f);

        GameObject bulletObj = Instantiate(prefab, transform.position, rot);

        // Fan.cs を再利用して方向と弾速を渡す
        Fan fanScript = bulletObj.GetComponent<Fan>();
        if (fanScript != null)
        {
            fanScript.direction = dir;
            fanScript.bulletSpeed = speed;
        }
    }

    // ポーズ対応の待機処理
    private IEnumerator WaitWithPause(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            if (timeManager != null && timeManager.IstimePassing)
            {
                timer += Time.deltaTime;
            }
            yield return null;
        }
    }
}
