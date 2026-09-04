using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f; //自機の動く速さ
    Rigidbody2D rb; //Rigidbody2Dコンポーネントの取得用
    [SerializeField] GameObject BulletPrefab; //弾のプレハブ（召喚できるオブジェクト）
    [SerializeField] private float bulletInterval = 0.1f; //弾を撃つ間隔
    private Coroutine shootLoop; //弾を継続的に発射するループ（コルーチン）の取得用

    private void Start() {
        rb = GetComponent<Rigidbody2D>(); //playerからRigidbody2Dコンポーネントを取得
    }

    public void OnMove(InputAction.CallbackContext callback) {
        Vector3 inputVelocity = callback.ReadValue<Vector2>(); //wasd又は方向キー入力をVector3で取得（z要素は0になる）
        rb.linearVelocity = inputVelocity * moveSpeed; //rigidbodyで現在の速度をmoveSpeedに比例して変更する
    }

    public void OnShoot(InputAction.CallbackContext callback) {
        if (callback.performed) {
            shootLoop = StartCoroutine(ShootLoop()); //ボタンが押された瞬間の場合、射撃ループを開始
        }

        if (callback.canceled) {
            if(shootLoop != null) StopCoroutine(shootLoop); //ボタンが離された瞬間の場合、射撃ループを停止
        }
    }

    private IEnumerator ShootLoop() {
        while (true) {
            Instantiate(BulletPrefab, transform.position, Quaternion.identity); //BulletPrefabをオブジェクトの位置に初期角度で生成
            yield return new WaitForSeconds(bulletInterval); //bulletInterval秒待つ
        }
    }
}
