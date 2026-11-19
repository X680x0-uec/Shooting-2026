using UnityEngine;

public class BossMove : MonoBehaviour
{
    // ボスが上から降りてきて定位置で止まったあと、倒されるまで左右に往復し続けるスクリプト
    [Header("定位置までの移動設定")]
    [SerializeField] private float stopY = 3.0f;          // 画面上部で停止するY座標
    [SerializeField] private float enterSmoothTime = 1.0f;  // 停止するまでの滑らかさ（秒）

    [Header("左右往復の設定")]
    [SerializeField] private float moveSpeed = 2.5f;      // 左右移動のスピード
    [SerializeField] private float leftLimitX = -5.0f;    // 左端の折り返しX座標
    [SerializeField] private float rightLimitX = 5.0f;    // 右端の折り返しX座標

    private TimeManager timeManager;
    private bool isArrived = false;
    private Vector3 currentVelocity = Vector3.zero;
    private int direction = 1; // 1: 右へ移動, -1: 左へ移動

    void Start()
    {
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
    }

    void Update()
    {
        if (timeManager != null && timeManager.IstimePassing)
        {
            if (!isArrived)
            {
                // ステージ側で指定された出現位置から、stopY の高さまで滑らかに降りてくる
                Vector3 targetPos = new Vector3(transform.position.x, stopY, transform.position.z);
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, enterSmoothTime);

                // 定位置に到達したら左右移動へ切り替える
                if (Mathf.Abs(transform.position.y - stopY) <= 0.05f)
                {
                    transform.position = targetPos;
                    isArrived = true;
                }
            }
            else
            {
                // 左右に往復移動し続ける（EnemyStraightDown と同じ Translate 方式）
                transform.Translate(direction * moveSpeed * Time.deltaTime, 0, 0);

                // 右端に到達したら左向きへ反転
                if (transform.position.x >= rightLimitX)
                {
                    transform.position = new Vector3(rightLimitX, transform.position.y, transform.position.z);
                    direction = -1;
                }
                // 左端に到達したら右向きへ反転
                else if (transform.position.x <= leftLimitX)
                {
                    transform.position = new Vector3(leftLimitX, transform.position.y, transform.position.z);
                    direction = 1;
                }
            }
        }
    }
}