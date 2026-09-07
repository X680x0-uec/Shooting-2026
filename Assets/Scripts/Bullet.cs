using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f; //弾の速さ
    private bool IsMoving = true;
    private float destroyTimer = 0;

    void Awake() {
    }

    void Update() {
        if (IsMoving) {
            transform.Translate(0, speed * Time.deltaTime, 0); //常に上方向へ移動
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, speed * Time.deltaTime);
            //進行方向に光線を飛ばすことで衝突判定。Colliderを使う実装と異なり、どれだけ速く弾を打ち出してもオブジェクトをすり抜けなくなる。
            destroyTimer += Time.deltaTime;
            if (hit.collider != null && hit.collider.CompareTag("Enemy")) {
                //敵（Enemyタグのついたもの）に当たった時の処理
                Destroy(gameObject);
            }
        }

        if (destroyTimer > 1.0f) DestroyBullet();
    }

    private void DestroyBullet() {
        //弾削除時の処理を記述（爆発など）
        Destroy(gameObject);
    }

    public void Stop() {
        IsMoving = false;
    }

    public void ChangeSpeed() {
        speed *= Random.Range(0.8f, 4f);
    }

    public void Move() {
        IsMoving = true;
    }
}
