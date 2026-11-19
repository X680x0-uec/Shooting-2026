using UnityEngine;

public class Fan : MonoBehaviour
{
    // 扇形に発射される敵の弾のスクリプト
    [HideInInspector] public Vector3 direction; // 進行方向（敵スクリプトから設定）
    [HideInInspector] public float bulletSpeed; // 敵側から弾速を指定する場合用

    [Header("弾の基本設定")]
    [SerializeField] private float defaultSpeed = 7f; // 単独使用時のデフォルト弾速
    [SerializeField] private float lifeTime = 6f;     // 画面外へ出た後の自動消滅時間（秒）

    private TimeManager timeManager;
    private float currentLifeTime = 0f;

    void Start()
    {
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
        // 敵側から弾速が指定されていなければデフォルト値を使う
        if (bulletSpeed <= 0f)
        {
            bulletSpeed = defaultSpeed;
        }
    }

    void Update()
    {
        if (timeManager != null && timeManager.IstimePassing)
        {
            // ワールド座標基準（Space.World）で指定方向へ移動させる
            transform.Translate(direction.normalized * bulletSpeed * Time.deltaTime, Space.World);

            // ポーズ対応の寿命カウント（一定時間経過で自動削除）
            currentLifeTime += Time.deltaTime;
            if (currentLifeTime >= lifeTime)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵の弾がプレイヤーに当たった時の処理
        if (collision.gameObject.CompareTag("PlayerCore"))
        {
            Destroy(this.gameObject);
        }
    }
}