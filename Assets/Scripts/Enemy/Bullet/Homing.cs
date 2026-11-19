using UnityEngine;

public class Homing : MonoBehaviour
{
    // 一定角度内に自機がいる間だけ向きを補正する誘導弾スクリプト
    [HideInInspector] public Vector3 direction; // 初期進行方向（敵スクリプトから設定可能）
    [HideInInspector] public float bulletSpeed; // 敵側から弾速を指定する場合用

    [Header("誘導弾の基本設定")]
    [SerializeField] private float defaultSpeed = 5f;     // 弾速
    [SerializeField] private float rotateSpeed = 120f;    // 1秒あたりの最大旋回角度（度）
    [SerializeField] private float homingAngle = 45f;     // 誘導が続く視野角（進行方向から左右それぞれ何度の範囲か）
    [SerializeField] private float lifeTime = 7f;         // 自動消滅時間（秒）

    private TimeManager timeManager;
    private PlayerController player;
    private float currentLifeTime = 0f;

    void Start()
    {
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
        player = GameObject.FindAnyObjectByType<PlayerController>();

        if (bulletSpeed <= 0f)
        {
            bulletSpeed = defaultSpeed;
        }

        // 敵側から初期方向が渡されていない場合は、出現時に自機方向を初期向きにする
        if (direction == Vector3.zero)
        {
            if (player != null)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
            else
            {
                direction = Vector3.down;
            }
        }
        else
        {
            direction = direction.normalized;
        }

        UpdateRotation();
    }

    void Update()
    {
        if (timeManager != null && timeManager.IstimePassing)
        {
            // ① 誘導処理：自機が「進行方向から homingAngle 度以内」にいる場合のみ向きを補正
            if (player != null)
            {
                Vector3 toPlayer = (player.transform.position - transform.position).normalized;
                float angleToPlayer = Vector2.Angle(direction, toPlayer);

                // 自機が誘導範囲（視野角）内にいるか判定
                if (angleToPlayer <= homingAngle)
                {
                    // 現在の進行方向(direction)から自機方向(toPlayer)へ、rotateSpeedの速さで徐々に回転させる
                    float maxRadiansDelta = rotateSpeed * Mathf.Deg2Rad * Time.deltaTime;
                    direction = Vector3.RotateTowards(direction, toPlayer, maxRadiansDelta, 0f).normalized;
                    UpdateRotation();
                }
            }

            // ② 現在の direction 方向へ前進（Fan.cs と同じ移動方式）
            transform.Translate(direction * bulletSpeed * Time.deltaTime, Space.World);

            // ③ 寿命カウント
            currentLifeTime += Time.deltaTime;
            if (currentLifeTime >= lifeTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // 進行方向に合わせて弾の見た目（Z軸回転）を更新する
    private void UpdateRotation()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // 上向き画像基準の補正
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCore"))
        {
            Destroy(this.gameObject);
        }
    }
}