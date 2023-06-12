using Services;
using UnityEngine;

public class Skill_Clone : Skill
{
    private readonly ShadowManager shadowManager;

    public Skill_Clone(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        shadowManager = ServiceLocator.Get<ShadowManager>();
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowManager.shadowInfo.missileCount += skillInfoData.missileCountIncrease1_Clone;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowManager.shadowInfo.missileCount -= skillInfoData.missileCountIncrease1_Clone;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowManager.shadowInfo.missileScale += skillInfoData.missileScaleIncrease_Clone;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowManager.shadowInfo.missileScale -= skillInfoData.missileScaleIncrease_Clone;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowManager.shadowInfo.missileCount += skillInfoData.missileCountIncrease2_Clone;
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowManager.shadowInfo.missileCount -= skillInfoData.missileCountIncrease2_Clone;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, CloneShadow);
    }

    protected override void OnRemoveSkillGrade4()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, CloneShadow);
    }

    private void CloneShadow(Shadow shadow)
    {
        if (!shadow.createdByClone && Random.Range(0f, 1f) < skillInfoData.cloneProbability_Clone)
        {
            shadowManager.CloneShadow(shadow.transform.position);
        }
    }
}
