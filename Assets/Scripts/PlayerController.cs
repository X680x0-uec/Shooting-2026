using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //このクラスで時間停止の処理を行うが、GameMangagerクラスなどを作ってそれに処理させたほうがよい可能性もある
    private bool IstimePassing = true; //時間が止まっているか
    [SerializeField] private float moveSpeed = 1f; //自機の動く速さ
    Rigidbody2D rb; //Rigidbody2Dコンポーネントの取得用
    [SerializeField] Bullet BulletPrefab; //弾のプレハブ（召喚できるオブジェクト）
    [SerializeField] private float bulletInterval = 0.1f; //弾を撃つ間隔
    private Coroutine shootLoop; //弾を継続的に発射するループ（コルーチン）の取得用
    private List<Bullet> FiredBullets = new List<Bullet>(); //自分が撃った弾のリスト
    [SerializeField] private Vector3 InitialFireOffset = Vector3.zero; //弾が出る場所の補正

    private void Start() {
        rb = GetComponent<Rigidbody2D>(); //playerからRigidbody2Dコンポーネントを取得
    }

    public void OnMove(InputAction.CallbackContext callback) {
        Vector3 inputVelocity = callback.ReadValue<Vector2>(); //wasd又は方向キー入力をVector3で取得（z要素は0になる）
        rb.linearVelocity = inputVelocity * moveSpeed; //rigidbodyで現在の速度をmoveSpeedに比例して変更する
    }

    public void OnShoot(InputAction.CallbackContext callback) {
        //射撃時の処理
        if (callback.performed) {
            shootLoop = StartCoroutine(ShootLoop()); //ボタンが押された瞬間の場合、射撃ループを開始
        }

        if (callback.canceled) {
            if(shootLoop != null) StopCoroutine(shootLoop); //ボタンが離された瞬間の場合、射撃ループを停止
        }
    }

    public void OnTimeStop(InputAction.CallbackContext callback) {
        //時間停止キーが押されたときの処理（pcはzkey）
        if (IstimePassing) {
            TimeStop();
        } else {
            TimePass();
        }
    }

    private void TimeStop() {
        foreach (Bullet bullet in FiredBullets) {
            //Sceneに存在するBullet全てを停止させる
            IstimePassing = false;
            bullet.Stop();
        }
    }

    private void TimePass() {
        foreach (Bullet bullet in FiredBullets) {
            //Sceneに存在するBullet全てを動かす
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
                Vector3 offset = InitialFireOffset * Random.Range(0.6f, 1.4f); //時間停止時に撃った弾は位置が乱れる
                bullet = Instantiate(BulletPrefab, transform.position + offset, Quaternion.identity);
                bullet.ChangeSpeed(); //時間停止時に撃った弾は速度が乱れる
                bullet.Stop();
            }
            FiredBullets.Add(bullet); //弾のリストに先ほど生成した弾を追加
            FiredBullets.RemoveAll(b => b == null); //弾のリストの中で、時間経過や敵との衝突で消えたものを削除
            yield return new WaitForSeconds(bulletInterval); //bulletInterval秒待つ
        }
    }
}
