using Services;
using System.Collections.Generic;

public class Skill_RoughSea : Skill
{
    private readonly ObjectManager objectManager;
    private readonly ShadowInfo shadowInfo;
    private readonly ShadowLauncherInfo shadowLauncherInfo;
    private readonly Dictionary<Shadow, ShadowWaveLauncher> shadowWaveLaunchers;

    private int penetrateCount;

    public Skill_RoughSea(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        objectManager = ServiceLocator.Get<ObjectManager>();
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
        shadowLauncherInfo = new ShadowLauncherInfo(skillInfoData.damage1_RoughSea,
            skillInfoData.waveInterval1_RoughSea, skillInfoData.watchRadius_RoughSea,
            skillInfoData.flyingSpeed_RoughSea, skillInfoData.shootRange_RoughSea);
        shadowWaveLaunchers = new Dictionary<Shadow, ShadowWaveLauncher>();
        penetrateCount = skillInfoData.penetrateCount_RoughSea;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowWaveLauncher1);
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, RemoveShadowWaveLauncher);
    }

    protected override void OnRemoveSkillGrade1()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateShadowWaveLauncher1);
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, RemoveShadowWaveLauncher);
    }

    private void GenerateShadowWaveLauncher1(Shadow shadow)
    {
        ShadowWaveLauncher waveLauncher = objectManager.Activate(
            EObject.ShadowWaveLauncher, shadow.centerTransform.position, shadow.transform)
            .Transform.GetComponent<ShadowWaveLauncher>();
        shadowLauncherInfo.missileCount = shadowInfo.missileCount;
        shadowLauncherInfo.missileScale = shadowInfo.missileScale;
        waveLauncher.Initialize(shadowLauncherInfo, penetrateCount);
        shadowWaveLaunchers.Add(shadow, waveLauncher);
    }

    private void RemoveShadowWaveLauncher(Shadow shadow)
    {
        if (shadowWaveLaunchers.ContainsKey(shadow))
        {
            shadowWaveLaunchers[shadow].DestroySelf();
            shadowWaveLaunchers.Remove(shadow);
        }
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowLauncherInfo.attackDamage = skillInfoData.damage2_RoughSea;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowLauncherInfo.attackDamage = skillInfoData.damage1_RoughSea;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        penetrateCount = int.MaxValue;
    }

    protected override void OnRemoveSkillGrade3()
    {
        penetrateCount = skillInfoData.penetrateCount_RoughSea;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowLauncherInfo.attackInterval = skillInfoData.waveInterval2_RoughSea;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowLauncherInfo.attackInterval = skillInfoData.waveInterval1_RoughSea;
    }
}
