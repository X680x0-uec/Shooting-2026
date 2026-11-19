using System.Collections;
using UnityEngine;

public class HomingShot : MonoBehaviour
{
    private PlayerController player;
    private TimeManager timeManager;
    private bool isShoot = false;
    private bool isFinished = false;
    private float currentTime = 0f;
    private Coroutine shootCoroutine;

    [Header("プレハブ・発射設定")]
    [SerializeField] private GameObject homingBulletPrefab; // HomingBullet.csが付いた弾プレハブ
    [SerializeField] private int bulletCount = 1;           // 一度に撃つ弾数（1なら自機へ真っ直ぐ誘導発射）
    [SerializeField] private float spreadAngle = 30f;       // 複数発撃つ場合の初期拡散角度
    [SerializeField] private float bulletSpeed = 5f;        // 弾速

    [Header("発射タイミング設定")]
    [SerializeField] private float interval = 1.5f;         // 発射間隔（秒）
    [SerializeField] private float startTime = 1.0f;        // 出現から発射開始までの時間（秒）
    [SerializeField] private float endTime = 7.0f;          // 出現から発射終了までの時間（秒）

    void Start()
    {
        player = GameObject.FindAnyObjectByType<PlayerController>();
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
    }

    void Update()
    {
        if (timeManager != null && timeManager.IstimePassing)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= startTime && !isShoot && !isFinished)
            {
                shootCoroutine = StartCoroutine(ShootLoop());
                isShoot = true;
            }

            if (currentTime >= endTime && isShoot && !isFinished)
            {
                if (shootCoroutine != null) StopCoroutine(shootCoroutine);
                isFinished = true;
            }
        }
    }

    private IEnumerator ShootLoop()
    {
        while (true)
        {
            if (timeManager.IstimePassing)
            {
                FireHoming();

                float waitTimer = 0f;
                while (waitTimer < interval)
                {
                    if (timeManager.IstimePassing) waitTimer += Time.deltaTime;
                    yield return null;
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    private void FireHoming()
    {
        if (homingBulletPrefab == null) return;

        Vector2 toPlayer = Vector2.down;
        if (player != null)
        {
            toPlayer = (player.transform.position - transform.position).normalized;
        }

        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        if (bulletCount <= 1)
        {
            SpawnHoming(baseAngle);
            return;
        }

        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = baseAngle - (spreadAngle / 2f);

        for (int i = 0; i < bulletCount; i++)
        {
            SpawnHoming(startAngle + (angleStep * i));
        }
    }

    private void SpawnHoming(float angleDegree)
    {
        float rad = angleDegree * Mathf.Deg2Rad;
        Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
        Quaternion rot = Quaternion.Euler(0f, 0f, angleDegree - 90f);

        GameObject bulletObj = Instantiate(homingBulletPrefab, transform.position, rot);

        Homing hb = bulletObj.GetComponent<Homing>();
        if (hb != null)
        {
            hb.direction = dir;
            hb.bulletSpeed = bulletSpeed;
        }
    }
}
