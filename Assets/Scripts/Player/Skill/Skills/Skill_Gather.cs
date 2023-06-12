using Services;

public class Skill_Gather : Skill
{
    private readonly ShadowChargeTimer shadowChargeTimer;
    private int chargedCount = 0;

    public Skill_Gather(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        shadowChargeTimer = ServiceLocator.Get<ShadowManager>().shadowChargeTimer;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowChargeTimer.ShadowChargeCD -= skillInfoData.chargeCDDecrease_Gather;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowChargeTimer.ShadowChargeCD -= skillInfoData.chargeCDDecrease_Gather;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowChargeTimer.OnShadowChargedCountChange += CheckChargeIncreaseMaxChargeCount;
        chargedCount = 0;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowChargeTimer.OnShadowChargedCountChange -= CheckChargeIncreaseMaxChargeCount;
    }

    private void CheckChargeIncreaseMaxChargeCount(int before, int after)
    {
        if (after > before && ++chargedCount == skillInfoData.needChargedCount_Gather)
        {
            shadowChargeTimer.MaxShadowChargeCount += skillInfoData.maxChargeCountIncrease1_Gather;
            shadowChargeTimer.OnShadowChargedCountChange -= CheckChargeIncreaseMaxChargeCount;
        }
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowChargeTimer.SetShadowPrecharge(true);
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowChargeTimer.SetShadowPrecharge(false);
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowChargeTimer.MaxShadowChargeCount += skillInfoData.maxChargeCountIncrease2_Gather;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowChargeTimer.MaxShadowChargeCount -= skillInfoData.maxChargeCountIncrease2_Gather;
    }
}
