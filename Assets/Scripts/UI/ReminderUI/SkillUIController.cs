using Services;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIController : UIController
{
    public List<Color> colors;
    public OwnedSkillHoverUI ownedSkillHoverUI;
    private GameObject skillUIPrefab;
    private List<Transform> skillUILines;
    private List<ReminderSkillUI> ownedSkillUIs;

    protected override void Start()
    {
        base.Start();
        if (colors == null || colors.Count != 4)
            Debug.LogError("SkillUIController的colors未设置正确");

        ownedSkillHoverUI = GetComponentInChildren<OwnedSkillHoverUI>();
        skillUIPrefab = Resources.Load<GameObject>("ReminderSkillUI");
        skillUILines = new List<Transform>();
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
            skillUILines.Add(transform.GetChild(0).GetChild(i));

        ownedSkillUIs = new List<ReminderSkillUI>();

        EventSystem eventSystem = ServiceLocator.Get<EventSystem>();
        eventSystem.AddListener<Skill>(EEvent.OnUpgradeSkill, OnUpgradeSkill);
        eventSystem.AddListener<Skill>(EEvent.OnRemoveSkill, OnRemoveSkill);
    }

    private void OnUpgradeSkill(Skill skill)
    {
        ReminderSkillUI skillUI = ownedSkillUIs.Find(x => x.skill == skill);
        if (skillUI == null)
        {
            //skillUI = Instantiate(skillUIPrefab, skillUILines[ownedSkillUIs.Count / 10])
            //    .GetComponent<ReminderSkillUI>();
            skillUI = Instantiate(skillUIPrefab, skillUILines[skill.isAttackSkill ? 0 : 1])
                .GetComponent<ReminderSkillUI>();
            skillUI.Initialize(skill, this);
            ownedSkillUIs.Add(skillUI);
        }
        else
        {
            skillUI.UpdateUI();
        }
    }

    private void OnRemoveSkill(Skill skill)
    {
        int skillIndex;
        for (skillIndex = 0; skillIndex < ownedSkillUIs.Count; skillIndex++)
        {
            if (ownedSkillUIs[skillIndex].skill == skill) break;
        }
        if (skillIndex < ownedSkillUIs.Count)
        {
            ReminderSkillUI skillUI = ownedSkillUIs[skillIndex];
            skillUI.DestroySelf();
            ownedSkillUIs.Remove(skillUI);
            //if (skillIndex < 10 && ownedSkillUIs.Count >= 10)
            //{
            //    skillUILines[1].GetChild(0).SetParent(skillUILines[0]);
            //}
        }
        else
        {
            Debug.LogError("试图移除未拥有的技能！");
        }
    }
}
