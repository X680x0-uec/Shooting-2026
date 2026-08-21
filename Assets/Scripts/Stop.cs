using UnityEngine;
using UnityEngine.InputSystem;

public class Stop : MonoBehaviour
{
    public bool _stop = false;
    private float timer = 0;
    public float stoptime = 5f;  //時間の停止時間
    [SerializeField] private InputAction stop;  //時止めのアクションを起こしたいキーの割り当てはインスペクターから
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stop.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (stop.WasPressedThisFrame() && !_stop)
        {
            timer = 0;
            _stop = true;
        }
        if (_stop)
        {
            timer += Time.deltaTime;
            if(timer >= stoptime)
            {
                _stop = false;

            }
        }
    }
}