using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    //このクラスで時間停止の処理を行うが、GameMangagerクラスなどを作ってそれに処理させたほうがよい可能性もある
    [SerializeField] private TimeManager timeManager; //時間が止まっているか
    [SerializeField] private float moveSpeed = 1f; //自機の動く速さ
    Rigidbody2D rb; //Rigidbody2Dコンポーネントの取得用
    [SerializeField] Bullet BulletPrefab; //弾のプレハブ（召喚できるオブジェクト）
    [SerializeField] private float bulletInterval = 0.1f; //弾を撃つ間隔
    private Coroutine shootLoop; //弾を継続的に発射するループ（コルーチン）の取得用
    private List<Bullet> FiredBullets = new List<Bullet>(); //自分が撃った弾のリスト
    [SerializeField] private Vector3 InitialFireOffset = Vector3.zero; //弾が出る場所の補正
    [SerializeField] private InputAction sllow;
    [SerializeField] PlayerHp playerHp;
    private int bulletNumWhileTimeStop = 1; //時間停止中に発射する弾の数
    [SerializeField] private UIManager uIManager;
    Vector3 inputVelocity;

    private void Start() {
        rb = GetComponent<Rigidbody2D>(); //playerからRigidbody2Dコンポーネントを取得
        if (timeManager == null) {
            timeManager = GameObject.FindAnyObjectByType<TimeManager>();
        }
        sllow.Enable();
    }

    void Update() {
        float currentMoveSpeed = moveSpeed;
        if (sllow.IsPressed()) {
            currentMoveSpeed *= 0.5f;
        }
        rb.linearVelocity = inputVelocity * currentMoveSpeed; //rigidbodyで現在の速度をcurrentMoveSpeedに比例して変更する
    }

    public void OnMove(InputAction.CallbackContext callback) {
        inputVelocity = callback.ReadValue<Vector2>(); //wasd又は方向キー入力をVector3で取得（z要素は0になる）
    }

    public void OnShoot(InputAction.CallbackContext callback) {
        if (Time.timeScale == 0f) return;
        //射撃時の処理
        if (callback.performed) {
            shootLoop = StartCoroutine(ShootLoop()); //ボタンが押された瞬間の場合、射撃ループを開始
        }

        if (callback.canceled) {
            if (shootLoop != null) StopCoroutine(shootLoop); //ボタンが離された瞬間の場合、射撃ループを停止
        }
    }

    public void OnPose(InputAction.CallbackContext callback) {
        uIManager.ShowPosePanel();
        if (Time.timeScale == 0f) {
            Time.timeScale = 1.0f;
            return;
        }
        Time.timeScale = 0f;
    }

    private IEnumerator ShootLoop() {
        while (true) {
            GameObject bullet;
            if (timeManager.IstimePassing) {
                Vector3 offset = InitialFireOffset;
                bullet = Instantiate(BulletPrefab, transform.position + offset, Quaternion.identity).gameObject;
            } else {
                Vector3 offset = InitialFireOffset * Random.Range(0.6f, 1.4f); //時間停止時に撃った弾は位置が乱れる
                bullet = Instantiate(BulletPrefab, transform.position + offset, Quaternion.identity).gameObject;
                Bullet bullet1 = bullet.GetComponent<Bullet>();
                BulletCommon bulletCommon = bullet.GetComponent<BulletCommon>();
                bulletCommon.ChangeSpeed(); //時間停止時に撃った弾は速度が乱れる
                bullet1.Stop();
            }
            FiredBullets.Add(bullet.GetComponent<Bullet>()); //弾のリストに先ほど生成した弾を追加
            FiredBullets.RemoveAll(b => b == null); //弾のリストの中で、時間経過や敵との衝突で消えたものを削除
            yield return new WaitForSeconds(bulletInterval); //bulletInterval秒待つ
        }
    }

    public void Enforce(int skillType, float value) {
        switch (skillType) {
            case 0:
                moveSpeed = value;
                break;
            case 1:
                bulletInterval = value;
                break;
            case 2:
                timeManager.stoptime = value;
                break;
            case 3:
                playerHp.ChangeHp(value);
                break;
            default:
                break;
        }
    }
}