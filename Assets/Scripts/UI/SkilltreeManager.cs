using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkilltreeManager : MonoBehaviour {
    private List<bool> skillActivationStatus = new List<bool>();
    [SerializeField] private List<SkillExplaination> skillData = new List<SkillExplaination>();

    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI detailedExplaination;
    [SerializeField] private Button ApplyButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI ApplyButtonText;

    private int showingSkillID = 0;
    private void Awake() {
        for (int i = 0; i < skillData.Count; i++) {
            skillActivationStatus.Add(false);
        }
    }

    public void ShowSkillExplaination(int skillID) {
        showingSkillID = skillID;
        if (skillActivationStatus[skillID]) {
            ApplyButton.interactable = false;
            ApplyButtonText.text = "取得済";
        } else if (IsOneBeforeSkillGained(skillID)) {
            ApplyButton.interactable = true;
            ApplyButtonText.text = "適用";
        } else {
            ApplyButton.interactable = false;
            ApplyButtonText.text = "---";
        }
        skillNameText.text = skillData[skillID].skillName;
        detailedExplaination.text = skillData[skillID].detailedExplaination;
    }

    private bool IsOneBeforeSkillGained(int skillID) {
        if (skillID % 4 == 1 || skillID == 0) {
            return true;
        } else if (skillActivationStatus[skillID - 1]) {
            return true;
        }
        return false;
    }

    public void ActivateSkill() {
        skillActivationStatus[showingSkillID] = true;
        ShowSkillExplaination(showingSkillID);
        ApplySkillEnforece(showingSkillID);
    }

    private void ApplySkillEnforece(int skillID) {
        if (skillData[skillID].enforceSkill == SkillExplaination.SkillType.Speed) {
            playerController.Enforce(0, skillData[skillID].enforceValue);
        } else if (skillData[skillID].enforceSkill == SkillExplaination.SkillType.ShootInterval) {
            playerController.Enforce(1, skillData[skillID].enforceValue);
        } else if (skillData[skillID].enforceSkill == SkillExplaination.SkillType.TimeStopLength) {
            playerController.Enforce(2, skillData[skillID].enforceValue);
        } else {
            playerController.Enforce(3, skillData[skillID].enforceValue);
        }
    }
}
