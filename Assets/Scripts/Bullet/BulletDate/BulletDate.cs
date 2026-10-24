using UnityEngine;

[CreateAssetMenu(fileName = "BulletDate", menuName = "Scriptable Objects/BulletDate")]
public class BulletDate : ScriptableObject
{
    // 弾の名前(使わないかも)、攻撃力、速度を設定(Player弾)。これから作られたScriptableObjectはそれぞれの弾へアタッチされたBulletCommonのbulletDateへいれる。
    public string Name; // 弾の名前
    public float Power; // 弾の攻撃力
    public float speed; // 弾の速度
}
