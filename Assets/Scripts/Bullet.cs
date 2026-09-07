using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; //弾の速さ
    private bool IsMoving = true; //弾が止まっている（時間停止状態）かどうかの判定
    private float destroyTimer = 0; //時間が動いている時だけ流れるタイマー。1.0を超えるとその弾はDestroyされる。
    [SerializeField] private SpriteRenderer mySprite; //自身のSpriteRendererコンポーネント参照用
    [SerializeField] private Sprite normalTexture; //普通時のテクスチャ
    [SerializeField] private Sprite timeStopTexture; //時間停止時のテクスチャ

    void Awake() {
        mySprite.sprite = normalTexture; //初期状態は普通時のテクスチャ
    }

    void Update() {
        if (IsMoving) {
            transform.Translate(0, speed * Time.deltaTime, 0); //常に上方向へ移動
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, speed * Time.deltaTime);
            //進行方向に光線を飛ばすことで衝突判定。Colliderを使う実装と異なり、どれだけ速く弾を打ち出してもオブジェクトをすり抜けなくなる。
            destroyTimer += Time.deltaTime; //時間のカウント
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) {
                //敵（Enemyタグのついたもの）に当たった時の処理
                Destroy(gameObject);
            }
        }

        if (destroyTimer > 1.0f) DestroyBullet(); //タイマーが1.0を上回るとDestoroy
    }

    private void DestroyBullet() {
        //弾削除時の処理を記述（爆発など）
        Destroy(gameObject);
    }

    public void Stop() {
        //時間停止時の処理（外部から呼ばれるためpublic）
        mySprite.sprite = timeStopTexture;
        IsMoving = false;
    }

    public void ChangeSpeed() {
        //速度変化の処理（外部から呼ばれるためpublic）
        speed *= Random.Range(0.8f, 4f);
    }

    public void Move() {
        //時間が流れる時の処理（外部から呼ばれるためpublic）
        mySprite.sprite = normalTexture;
        IsMoving = true;
    }
}
