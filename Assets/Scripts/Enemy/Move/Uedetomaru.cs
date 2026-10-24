using UnityEngine;

public class Uedetomaru : MonoBehaviour
{
    // 上から降ってきた敵を画面上部で滑らかに止め、一定時間後上へ帰るスクリプト
    private TimeManager timeManager;
    private float currentTime;
    private bool firstMove = false;
    private Vector3 beforevel = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(timeManager == null){
            timeManager = GameObject.FindAnyObjectByType<TimeManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeManager.IstimePassing){
            currentTime += Time.deltaTime;
            if (!firstMove){
                transform.position = Vector3.SmoothDamp(this.transform.position, new Vector3(this.transform.position.x, 3, 0), ref beforevel, 1f);
            }
            if(transform.position.y <= 3.1 && !firstMove){
                currentTime = 0;
                firstMove = true;
            }
            if(firstMove && currentTime >= 5f){
                transform.position = Vector3.SmoothDamp(this.transform.position, new Vector3(this.transform.position.x, 6, 0), ref beforevel, 1f);
            }
            if(firstMove && transform.position.y >= 5.9){
                Destroy(this.gameObject);
            }
        }
    }
}
