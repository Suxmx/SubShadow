using Services;
using System.Collections.Generic;

public class Skill_ConfusedNoise : Skill
{
    private readonly ShadowInfo shadowInfo;
    private readonly ObjectManager objectManager;
    private readonly Dictionary<Shadow, VortexTrigger> vortexTriggers;
    private bool isAttracter;

    public Skill_ConfusedNoise(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
        objectManager = ServiceLocator.Get<ObjectManager>();
        vortexTriggers = new Dictionary<Shadow, VortexTrigger>();
        isAttracter = false;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowInfo.stayingDuration += skillInfoData.stayingDurationIncrease1_ConfusedNoise;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowInfo.stayingDuration -= skillInfoData.stayingDurationIncrease1_ConfusedNoise;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, GenerateVortex);
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, RemoveVortex);
    }

    protected override void OnRemoveSkillGrade2()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, GenerateVortex);
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, RemoveVortex);
        foreach (var vortexTrigger in vortexTriggers.Values)
        {
            vortexTrigger.DestroySelf();
        }
        vortexTriggers.Clear();
    }

    private void GenerateVortex(Shadow shadow)
    {
        VortexTrigger vortexTrigger = objectManager.Activate(EObject.VortexTrigger,
            shadow.centerTransform.position, shadow.transform).Transform
            .GetComponent<VortexTrigger>();
        vortexTrigger.Initialize(-skillInfoData.decelerateMultiplier_ConfusedNoise, 
            skillInfoData.decelerateRadius_ConfusedNoise, isAttracter);
        vortexTriggers.Add(shadow, vortexTrigger);
    }

    private void RemoveVortex(Shadow shadow)
    {
        if (vortexTriggers.ContainsKey(shadow))
        {
            vortexTriggers[shadow].DestroySelf();
            vortexTriggers.Remove(shadow);
        }
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowInfo.stayingDuration += skillInfoData.stayingDurationIncrease2_ConfusedNoise;
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowInfo.stayingDuration -= skillInfoData.stayingDurationIncrease2_ConfusedNoise;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        isAttracter = true;
    }

    protected override void OnRemoveSkillGrade4()
    {
        isAttracter = false;
    }
}
