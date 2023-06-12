using Services;
using UnityEngine;

public class Skill_Ghost : Skill
{
    private readonly ShadowInfo shadowInfo;

    public Skill_Ghost(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowInfo.stayingDuration -= skillInfoData.stayingDurationDecrease1_Ghost;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowInfo.stayingDuration += skillInfoData.stayingDurationDecrease1_Ghost;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowInfo.flyingSpeed += skillInfoData.flyingSpeedIncrease1_Ghost;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowInfo.flyingSpeed -= skillInfoData.flyingSpeedIncrease1_Ghost;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowInfo.stayingDuration -= skillInfoData.stayingDurationDecrease2_Ghost;
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowInfo.stayingDuration += skillInfoData.stayingDurationDecrease2_Ghost;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowInfo.flyingSpeed += skillInfoData.flyingSpeedIncrease2_Ghost;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowInfo.flyingSpeed -= skillInfoData.flyingSpeedIncrease2_Ghost;
    }
}
