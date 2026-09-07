using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool IstimePassing = true;
    [SerializeField] private float moveSpeed = 1f; //自機の動く速さ
    Rigidbody2D rb; //Rigidbody2Dコンポーネントの取得用
    [SerializeField] Bullet BulletPrefab; //弾のプレハブ（召喚できるオブジェクト）
    [SerializeField] private float bulletInterval = 0.1f; //弾を撃つ間隔
    private Coroutine shootLoop; //弾を継続的に発射するループ（コルーチン）の取得用
    private List<Bullet> FiredBullets = new List<Bullet>();
    [SerializeField] private Vector3 InitialFireOffset = Vector3.zero;

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

    public void OnTimeStop(InputAction.CallbackContext callback) {
        if (IstimePassing) {
            TimeStop();
        } else {
            TimePass();
        }
    }

    private void TimeStop() {
        foreach (Bullet bullet in FiredBullets) {
            IstimePassing = false;
            bullet.Stop();
        }
    }

    private void TimePass() {
        foreach (Bullet bullet in FiredBullets) {
            IstimePassing = true;
            bullet.Move();
        }
    }

    private IEnumerator ShootLoop() {
        while (true) {
            Bullet bullet;
            if (IstimePassing) {
                Vector3 offset = InitialFireOffset;
                bullet = Instantiate(BulletPrefab, transform.position + offset, Quaternion.identity);
            } else {
                Vector3 offset = InitialFireOffset * Random.Range(0.6f, 1.4f);
                bullet = Instantiate(BulletPrefab, transform.position + offset, Quaternion.identity);
                bullet.ChangeSpeed();
                bullet.Stop();
            }
            FiredBullets.Add(bullet);
            FiredBullets.RemoveAll(b => b == null);
            yield return new WaitForSeconds(bulletInterval); //bulletInterval秒待つ
        }
    }
}
