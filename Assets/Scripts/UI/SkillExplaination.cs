using UnityEngine;

[CreateAssetMenu(fileName = "SkillExplaination", menuName = "Scriptable Objects/SkillExplaination")]
public class SkillExplaination : ScriptableObject {
    public int skillID = 0;
    public string skillName = "";
    public string detailedExplaination = "";
}
