using Services;
using UnityEngine;

public class Skill_Unbridled : Skill
{
    private readonly ShadowManager shadowManager;

    public Skill_Unbridled(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        shadowManager = ServiceLocator.Get<ShadowManager>();
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowManager.shadowInfo.basicScale += skillInfoData.scaleIncrease_Unbridled;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowManager.shadowInfo.basicScale -= skillInfoData.scaleIncrease_Unbridled;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowManager.ShadowSetDist += skillInfoData.setRadiusIncrease_Unbridled;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowManager.ShadowSetDist -= skillInfoData.setRadiusIncrease_Unbridled;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        eventSystem.AddListener<Shadow, Collider2D>(EEvent.OnShadowTrigger, ShadowCauseVertigo);
    }

    protected override void OnRemoveSkillGrade3()
    {
        eventSystem.RemoveListener<Shadow, Collider2D>(EEvent.OnShadowTrigger, ShadowCauseVertigo);
    }

    private void ShadowCauseVertigo(Shadow shadow, Collider2D collision)
    {
        if (shadow.ShadowState == ShadowState.Setting || shadow.ShadowState == ShadowState.Recalling 
            && collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            if (enemy != null && enemy.IsAlive)
            {
                enemy.GetVertigo(skillInfoData.vertigoDuration_Unbridled);
            }
        }
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowManager.shadowInfo.flyingDistScaleMultiplier = skillInfoData.flyingDistScaleMultiplier_Unbridled;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowManager.shadowInfo.flyingDistScaleMultiplier = 0f;
    }
}
