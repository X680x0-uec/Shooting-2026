using UnityEngine;

public class BulletCommon : MonoBehaviour
{
    // Player弾の移動と当たり判定の処理
    // BulletCommonとかいう名前を付けてしまいましたが、弾の種類を増やすなら共通処理ではなくなるので、わかりやすさのために名前は変える必要があるかも
    [SerializeField] private BulletDate bulletDate; // 弾の攻撃力や速度のデータ
    private float speed; // ScriptableObjectは違うオブジェクトが同じところを参照するので、データのspeedをこの中で使えるようにする
    private float power; // 上と同様
    private float destroyTimer = 0; //時間が動いている時だけ流れるタイマー。1.0を超えるとその弾はDestroyされる。
    private TimeManager timeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
        speed = bulletDate.speed;
        power = bulletDate.Power;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.IstimePassing)
        {
            Debug.Log(speed);
            transform.Translate(0, speed * Time.deltaTime, 0); //常に上方向へ移動
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, speed * Time.deltaTime);
            //進行方向に光線を飛ばすことで衝突判定。Colliderを使う実装と異なり、どれだけ速く弾を打ち出してもオブジェクトをすり抜けなくなる。
            destroyTimer += Time.deltaTime; //時間のカウント
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) {
                //敵（Enemyタグのついたもの）に当たった時の処理
                Destroy(gameObject);
                hit.collider.GetComponent<EnemyHPManager>().TakeDamege(power); // 敵のEnemyHPManagerというスクリプトが持つTakeDamageメソッドを引数にpowerを渡して実行する
            }
        }
        if (destroyTimer > 1.0f) DestroyBullet(); //タイマーが1.0を上回るとDestoroy
    }

    private void DestroyBullet() {
        //弾削除時の処理を記述（爆発など）
        Destroy(gameObject);
    }
    public void ChangeSpeed() {
        //速度変化の処理（外部から呼ばれるためpublic）
        speed = Random.Range(speed * 0.8f, speed * 4f);
        Debug.Log(speed);
    }
}
