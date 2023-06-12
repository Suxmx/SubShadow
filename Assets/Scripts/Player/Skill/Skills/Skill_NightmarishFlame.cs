using Services;
using UnityEngine;

public class Skill_NightmarishFlame : Skill
{
    private readonly FireInfo fireInfo;
    private readonly ObjectManager objectManager;
    private readonly ShadowManager shadowManager;
    private readonly Transform playerTransform;

    public Skill_NightmarishFlame(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true, true)
    {
        fireInfo = new FireInfo
        {
            fireDuration = skillInfoData.burnDuration_NightmarishFlame,
            judgeInterval = skillInfoData.damageInterval_NightmarishFlame,
            damage = skillInfoData.burnDamage_NightmarishFlame,
            hpDamageMulitiplier = 0f,
            playerStatusInfo = ServiceLocator.Get<Player>().PlayerStatusInfo,
        };
        objectManager = ServiceLocator.Get<ObjectManager>();
        shadowManager = ServiceLocator.Get<ShadowManager>();
        playerTransform = ServiceLocator.Get<Player>().transform;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        eventSystem.AddListener<EnemyStatusInfo, IDamager>(EEvent.OnHurtEnemy, BurnEnemy);
    }

    protected override void OnRemoveSkillGrade1()
    {
        eventSystem.RemoveListener<EnemyStatusInfo, IDamager>(EEvent.OnHurtEnemy, BurnEnemy);
    }

    private void BurnEnemy(EnemyStatusInfo enemyStatusInfo, IDamager damager)
    {
        ShadowDamager shadowDamager = damager as ShadowDamager;
        if (shadowDamager != null && shadowDamager.shadow.ShadowState == ShadowState.Recalling)
        {
            enemyStatusInfo.GetFired(fireInfo);
        }
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowManager.OnSetShadow += GenerateFireTrigger;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowManager.OnSetShadow -= GenerateFireTrigger;

    }

    private void GenerateFireTrigger(Shadow shadow)
    {
        objectManager.Activate(EObject.FireTrigger, playerTransform.position, playerTransform).Transform
            .GetComponent<FireTrigger>().Initialize(fireInfo, skillInfoData.burnRadius_NightmarishFlame);
    }

    protected override void OnUpgradeSkillGrade3()
    {
        fireInfo.hpDamageMulitiplier = skillInfoData.hpDamageMulitiplier_NightmarishFlame;
    }

    protected override void OnRemoveSkillGrade3()
    {
        fireInfo.hpDamageMulitiplier = 0f;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        fireInfo.judgeInterval *= skillInfoData.damageIntervalMulitiplier_NightmarishFlame;
    }

    protected override void OnRemoveSkillGrade4()
    {
        fireInfo.judgeInterval = skillInfoData.damageInterval_NightmarishFlame;
    }
}
