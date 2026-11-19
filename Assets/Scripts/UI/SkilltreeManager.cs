using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkilltreeManager : MonoBehaviour {
    private List<bool> skillActivationStatus = new List<bool>();
    [SerializeField] private List<SkillExplaination> skillData = new List<SkillExplaination>();

    [SerializeField] private TextMeshProUGUI skillNameText;
    [SerializeField] private TextMeshProUGUI detailedExplaination;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private Button ApplyButton;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI ApplyButtonText;
    private int currentPoint = 0;
    [SerializeField] private float pointInterval = 2f;

    private int showingSkillID = 0;
    private void Awake() {
        currentPoint = 0;
        for (int i = 0; i < skillData.Count; i++) {
            skillActivationStatus.Add(false);
        }
    }

    private void Start() {
        StartCoroutine(PointLoop());
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
        if (skillData[showingSkillID].cost > currentPoint) return;
        currentPoint -= skillData[showingSkillID].cost;
        RefleshPointText();
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

    private IEnumerator PointLoop() {
        while (true) {
            yield return new WaitForSeconds(pointInterval);
            currentPoint += 10;
            RefleshPointText();
        }
    }

    private void RefleshPointText() {
        pointText.text = "スキルポイント:" + currentPoint;
    }
}
