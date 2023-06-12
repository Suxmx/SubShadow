using Services;

public class Skill_Reap : Skill
{
    private readonly ShadowInfo shadowInfo;

    public Skill_Reap(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowInfo.recallDamageMultiplier = skillInfoData.reapDamageMultiplier_Reap;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowInfo.recallDamageMultiplier = 1f;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowInfo.scaleDamageMultiplier = skillInfoData.scaleDamageMultiplier_Reap;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowInfo.scaleDamageMultiplier = 0f;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowInfo.reflectFlyingDist = skillInfoData.reflectFlyingDist_Reap;
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowInfo.reflectFlyingDist = 0f;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowInfo.gradeMultiplier = skillInfoData.gradeMultiplier_Reap;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowInfo.gradeMultiplier = 0f;
    }
}
