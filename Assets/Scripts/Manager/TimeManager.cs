using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour
{
    // 時止めを管理するスクリプト
    public bool IstimePassing = true;
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
        if (stop.WasPressedThisFrame() && IstimePassing)
        {
            timer = 0;
            IstimePassing = false;
        }
        if (!IstimePassing)
        {
            timer += Time.deltaTime;
            if(timer >= stoptime)
            {
                IstimePassing = true;

            }
        }
    }
}