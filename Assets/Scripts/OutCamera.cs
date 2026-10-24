using UnityEngine;

public class OutCamera : MonoBehaviour
{
    //　敵や弾が画面外に出たら破壊(現時点)するスクリプト
    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
