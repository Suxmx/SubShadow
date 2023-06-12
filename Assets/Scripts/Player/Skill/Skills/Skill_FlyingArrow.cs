using Services;
using System.Collections.Generic;

public class Skill_FlyingArrow : Skill
{
    private readonly ObjectManager objectManager;
    private readonly ShadowInfo shadowInfo;
    private readonly ShadowLauncherInfo shadowLauncherInfo;
    private readonly Dictionary<Shadow, ShadowArrowLauncher> shadowArrowLaunchers;

    private int missileCountIncrease;

    public Skill_FlyingArrow(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        objectManager = ServiceLocator.Get<ObjectManager>();
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
        shadowLauncherInfo = new ShadowLauncherInfo(skillInfoData.damage1_FlyingArrow, 
            skillInfoData.arrowInterval_FlyingArrow, skillInfoData.watchRadius_FlyingArrow, 
            skillInfoData.flyingSpeed_FlyingArrow, skillInfoData.shootRange_FlyingArrow);
        shadowArrowLaunchers = new Dictionary<Shadow, ShadowArrowLauncher>();
        missileCountIncrease = skillInfoData.missileCountIncrease1_FlyingArrow;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowArrowLauncher1);
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, RemoveShadowArrowLauncher);
    }

    protected override void OnRemoveSkillGrade1()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowArrowLauncher1);
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, RemoveShadowArrowLauncher);
        foreach (var shadowArrowLauncher in shadowArrowLaunchers.Values)
        {
            shadowArrowLauncher.DestroySelf();
        }
        shadowArrowLaunchers.Clear();
    }

    private void GenerateShadowArrowLauncher1(Shadow shadow)
    {
        ShadowArrowLauncher arrowLauncher = objectManager.Activate(
            EObject.ShadowArrowLauncher, shadow.centerTransform.position, shadow.transform)
            .Transform.GetComponent<ShadowArrowLauncher>();
        shadowLauncherInfo.missileCount = missileCountIncrease + shadowInfo.missileCount;
        shadowLauncherInfo.missileScale = shadowInfo.missileScale;
        arrowLauncher.Initialize(shadowLauncherInfo);
        shadowArrowLaunchers.Add(shadow, arrowLauncher);
    }

    private void RemoveShadowArrowLauncher(Shadow shadow)
    {
        if (shadowArrowLaunchers.ContainsKey(shadow))
        {
            shadowArrowLaunchers[shadow].DestroySelf();
            shadowArrowLaunchers.Remove(shadow);
        }
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowLauncherInfo.attackDamage = skillInfoData.damage2_FlyingArrow;
        missileCountIncrease += skillInfoData.missileCountIncrease2_FlyingArrow;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowLauncherInfo.attackDamage = skillInfoData.damage1_FlyingArrow;
        missileCountIncrease -= skillInfoData.missileCountIncrease2_FlyingArrow;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowArrowLauncher1);
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowArrowLauncher3);
    }

    protected override void OnRemoveSkillGrade3()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowArrowLauncher3);
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowArrowLauncher1);
    }

    private void GenerateShadowArrowLauncher3(Shadow shadow)
    {
        ShadowArrowLauncher arrowLauncher = objectManager.Activate(
            EObject.ShadowArrowLauncher, shadow.centerTransform.position, shadow.transform)
            .Transform.GetComponent<ShadowArrowLauncher>();
        shadowLauncherInfo.missileCount = missileCountIncrease == int.MaxValue ? int.MaxValue 
            : missileCountIncrease + shadowInfo.missileCount;
        shadowLauncherInfo.missileScale = shadowInfo.missileScale;
        arrowLauncher.Initialize(shadowLauncherInfo, skillInfoData.arrowIntervalUpperLimit_FlyingArrow, 
            skillInfoData.arrowIntervalLowerLimit_FlyingArrow, shadow.lastingTimer);
        shadowArrowLaunchers.Add(shadow, arrowLauncher);
    }

    protected override void OnUpgradeSkillGrade4()
    {
        missileCountIncrease = int.MaxValue;
    }

    protected override void OnRemoveSkillGrade4()
    {
        missileCountIncrease = skillInfoData.missileCountIncrease1_FlyingArrow + skillInfoData.missileCountIncrease2_FlyingArrow;
    }
}
