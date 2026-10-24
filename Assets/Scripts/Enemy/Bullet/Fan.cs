using UnityEngine;

public class Fan : MonoBehaviour
{
    // 扇形に発射される敵の弾のスクリプト
    [HideInInspector] public Vector3 direction; // 方向は敵オブジェクトが指定するので、publicだけどインスペクターには表示しない
    private TimeManager timeManager;
    [SerializeField] private float speed = 7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.IstimePassing)
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 敵の弾がプレイヤーに当たった時の処理(弾のスクリプト全部に書くと面倒なので、まとめるかも)
        if (collision.gameObject.CompareTag("PlayerCore")){
            Destroy(this.gameObject);
        }
    }
}
