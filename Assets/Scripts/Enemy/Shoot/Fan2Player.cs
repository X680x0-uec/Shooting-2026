using System.Collections;
using UnityEngine;

public class Fan2Player : MonoBehaviour
{
    // 扇形 +　プレイヤーの方向に向かって弾を飛ばす敵にアタッチする用のスクリプト
    // 多用するようならShoot()の中のfor文を変える必要がある(現在は弾は5発しか出ないため、一般化する必要があるかも)
    private PlayerController player;
    private TimeManager timeManager;
    private bool isShoot = false;
    private float currentTime;
    private Vector2 offset = Vector2.down;
    private Coroutine shoot;
    [SerializeField] private float interval = 0.6f;
    [SerializeField] private GameObject EnemyBullet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        player = GameObject.FindAnyObjectByType<PlayerController>();
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update(){
        if (timeManager.IstimePassing)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= 1 && !isShoot)
            {
                shoot = StartCoroutine(Shootroop());
                isShoot = true;
            }
            if(currentTime >= 7f && isShoot)
            {
                StopCoroutine(shoot);
            }
        }
    }

    private IEnumerator Shootroop(){
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(interval);
        }
    }

    private void Shoot(){
        if(!timeManager.IstimePassing) return;
        if(player != null){
            offset = this.transform.position - player.transform.position;
        }
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg - 20;
        for(int i=0; i<5; i++)
        {
            float rad = angle * Mathf.Deg2Rad;
            float x = Mathf.Cos(rad);
            float y = Mathf.Sin(rad);
            Fan fan = Instantiate(EnemyBullet, this.transform.position, Quaternion.identity).GetComponent<Fan>();
            fan.direction = -Vector2.Normalize(new Vector2(x, y));
            angle += 10;
        }
    }
}
