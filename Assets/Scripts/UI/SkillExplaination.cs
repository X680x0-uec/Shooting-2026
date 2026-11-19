using UnityEngine;

[CreateAssetMenu(fileName = "SkillExplaination", menuName = "Scriptable Objects/SkillExplaination")]
public class SkillExplaination : ScriptableObject {
    public int skillID = 0; //各スキルごとに割り振られたID
    public string skillName = ""; //スキルの名前『速度強化1』など
    [TextArea(3, 6)]
    public string detailedExplaination = ""; //スキルの詳しい説明文
    public SkillType enforceSkill = SkillType.Speed; //強化対象のステータス、デフォルトはスピード強化
    public float enforceValue = 0; //強化の程度、

    public enum SkillType {
        Speed,
        Hp,
        ShootInterval,
        TimeStopLength
    }
}
