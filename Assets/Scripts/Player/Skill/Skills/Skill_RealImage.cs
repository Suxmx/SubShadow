using Services;

public class Skill_RealImage : Skill
{
    private readonly ShadowInfo shadowInfo;

    public Skill_RealImage(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowInfo.basicDamage += skillInfoData.basicDamageIncrease1_RealImage;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowInfo.basicDamage -= skillInfoData.basicDamageIncrease1_RealImage;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowInfo.basicDamage += skillInfoData.basicDamageIncrease2_RealImage;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowInfo.basicDamage -= skillInfoData.basicDamageIncrease2_RealImage;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowInfo.basicDamageMultiplier += skillInfoData.basicDamageMultiplierIncrease1_RealImage;
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowInfo.basicDamageMultiplier -= skillInfoData.basicDamageMultiplierIncrease1_RealImage;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowInfo.basicDamageMultiplier += skillInfoData.basicDamageMultiplierIncrease2_RealImage;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowInfo.basicDamageMultiplier -= skillInfoData.basicDamageMultiplierIncrease2_RealImage;
    }
}
