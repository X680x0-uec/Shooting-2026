using UnityEngine;

public class EnemyStraightDown : MonoBehaviour
{
    // 敵が上から真下に落ちてくるときのスクリプト
    [SerializeField] private float speed = 3f;
    private TimeManager timeManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManager = GameObject.FindAnyObjectByType<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(timeManager.IstimePassing) transform.Translate(0, -speed * Time.deltaTime, 0);
    }
}
