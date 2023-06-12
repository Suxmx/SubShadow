using UnityEngine;
using UnityEngine.UI;

public class ReminderSkillUI : MonoBehaviour
{
    public Skill skill;
    private Image image;
    private SkillUIController skillUIController;
    private ReminderSkillTargetUI reminderSkillTargetUI;

    private void Awake()
    {
        image = GetComponent<Image>();
        reminderSkillTargetUI = GetComponentInChildren<ReminderSkillTargetUI>();
    }

    public void Initialize(Skill skill, SkillUIController skillUIController)
    {
        reminderSkillTargetUI.skill = this.skill = skill;
        this.skillUIController = skillUIController;
        reminderSkillTargetUI.ownedSkillHoverUI = skillUIController.ownedSkillHoverUI;
        UpdateUI();
    }

    public void UpdateUI()
    {
        image.sprite = skill.skillInfo.icon;
        image.color = skillUIController.colors[Mathf.Min(skill.ownedGrade, 4) - 1];
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
