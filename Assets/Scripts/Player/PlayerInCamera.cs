using UnityEngine;

public class PlayerInCamera : MonoBehaviour
{
    // プレイヤーをカメラの外に出られないようにするスクリプト。画面サイズを変えるときはこれも変える必要がありそう
    private Camera maincamera;
    private float height;
    private float width;
    [SerializeField] private float margin = 0.3f; // 余白
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maincamera = Camera.main;
        height = maincamera.orthographicSize; // カメラの高さの半分の長さをheightにいれている
        width = height * maincamera.aspect; // 比率に合わせてwidthに長さをいれている
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.transform.position;
        Vector3 cameraPos = maincamera.transform.position;
        pos.x = Mathf.Clamp(pos.x, cameraPos.x - width + margin, cameraPos.x + width - margin); // プレイヤーのx座標が何以上何以下かを指定
        pos.y = Mathf.Clamp(pos.y, cameraPos.y - height + margin, cameraPos.y + height - margin); // プレイヤーのy座標が何以上何以下かを指定
        transform.position = pos;
    }
}
