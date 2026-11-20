using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeManager : MonoBehaviour {
    // 時止めを管理するスクリプト
    public bool IstimePassing = true;
    private bool IstimeStopPossible = false;
    private float timer = 0;
    public float stoptime = 1f;  //時間の停止時間
    public float timeStopCooltime = 3f; //クールタイム
    [SerializeField] TextMeshProUGUI timestopText;
    [SerializeField] private InputAction stop;  //時止めのアクションを起こしたいキーの割り当てはインスペクターから
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        stop.Enable();
        timer = 999f;
        IstimeStopPossible = true;
    }

    // Update is called once per frame
    void Update() {
        timer += Time.deltaTime;
        if (timer > stoptime + timeStopCooltime) {
            IstimeStopPossible = true;
            timestopText.text = "TimeStop:O";
        } else {
            IstimeStopPossible = false;
            timestopText.text = "TimeStop:X";
        }

        if (stop.WasPressedThisFrame() && IstimePassing && IstimeStopPossible) {
            timer = 0;
            IstimePassing = false;
        }
        if (!IstimePassing) {
            if (timer >= stoptime) {
                IstimePassing = true;
            }
        }
    }
}