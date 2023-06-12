using Services;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Sunspot : Skill
{
    private readonly ShadowInfo shadowInfo;
    private readonly ObjectManager objectManager;
    private readonly HashSet<Shadow> kickedShadows;
    protected readonly AudioManager audioManager;

    public Skill_Sunspot(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        shadowInfo = ServiceLocator.Get<ShadowManager>().shadowInfo;
        objectManager = ServiceLocator.Get<ObjectManager>();
        audioManager=ServiceLocator.Get<AudioManager>();
        kickedShadows = new HashSet<Shadow>();
    }

    protected override void OnUpgradeSkillGrade1()
    {
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, CauseExplodeBeforeRecall);
    }

    protected override void OnRemoveSkillGrade1()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, CauseExplodeBeforeRecall);
    }

    private void CauseExplodeBeforeRecall(Shadow shadow)
    {
        objectManager.Activate(EObject.ExplodeTrigger, shadow.centerTransform.position)
            .Transform.GetComponent<ExplodeTrigger>().Initialize(
            skillInfoData.recallDamageMultiplier_Sunspot * shadowInfo.Damage,
            skillInfoData.recallDamageRadius_Sunspot);
        audioManager.PlaySound("SunSpot_Explode",AudioPlayMode.Plenty);
    }

    protected override void OnUpgradeSkillGrade2()
    {
        eventSystem.AddListener<Shadow, Collider2D>(EEvent.OnShadowTrigger, CheckAndKickShadow);
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, RemoveKickedShadow);
    }

    protected override void OnRemoveSkillGrade2()
    {
        eventSystem.RemoveListener<Shadow, Collider2D>(EEvent.OnShadowTrigger, CheckAndKickShadow);
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, RemoveKickedShadow);
        kickedShadows.Clear();
    }

    private void CheckAndKickShadow(Shadow shadow, Collider2D collision)
    {
        if (shadow.ShadowState == ShadowState.Staying && collision.CompareTag("Player") 
            && !kickedShadows.Contains(shadow))
        {
            audioManager.PlaySound("SunSpot_TouchShadow",AudioPlayMode.Plenty);
            kickedShadows.Add(shadow);
            shadow.GetKicked(skillInfoData.pushDist_Sunspot, skillInfoData.pushSpeed_Sunspot,
                skillInfoData.pushDamageMultiplier_Sunspot);
        }
    }

    private void RemoveKickedShadow(Shadow shadow)
    {
        kickedShadows.Remove(shadow);
    }

    protected override void OnUpgradeSkillGrade3()
    {
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, CauseExplodeAfterCreate);
    }

    protected override void OnRemoveSkillGrade3()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, CauseExplodeAfterCreate);
    }

    private void CauseExplodeAfterCreate(Shadow shadow)
    {
        CauseExplodeGrade3(shadow.centerTransform.position);
    }

    private void CauseExplodeGrade3(Vector3 pos)
    {
        audioManager.PlaySound("SunSpot_Explode",AudioPlayMode.Plenty);
        objectManager.Activate(EObject.ExplodeTrigger, pos)
            .Transform.GetComponent<ExplodeTrigger>().Initialize(
            skillInfoData.stopDamageMultiplier_Sunspot * shadowInfo.Damage,
            skillInfoData.stopDamageRadius_Sunspot);
    }

    protected override void OnUpgradeSkillGrade4()
    {
        eventSystem.AddListener<Shadow>(EEvent.BeforeRecallShadow, GenerateExplodeSeedBeforeRecall);
    }

    protected override void OnRemoveSkillGrade4()
    {
        eventSystem.RemoveListener<Shadow>(EEvent.BeforeRecallShadow, GenerateExplodeSeedBeforeRecall);
    }

    private void GenerateExplodeSeedBeforeRecall(Shadow shadow)
    {
        objectManager.Activate(EObject.ExplodeSeed, shadow.centerTransform.position)
            .Transform.GetComponent<ExplodeSeed>().Initialize(
            skillInfoData.explodeSeedStayingTime_Sunspot, CauseExplodeGrade3);
    }
}
