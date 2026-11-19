using UnityEngine;

[CreateAssetMenu(fileName = "SkillExplaination", menuName = "Scriptable Objects/SkillExplaination")]
public class SkillExplaination : ScriptableObject {
    public int skillID = 0;
    public string skillName = "";
    [TextArea(3, 6)]
    public string detailedExplaination = "";
    public SkillType enforceSkill = SkillType.Speed;
    public float enforceValue = 0;

    public enum SkillType {
        Speed,
        Hp,
        ShootInterval,
        TimeStopLength
    }
}
