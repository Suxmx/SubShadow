using MyTimer;
using Services;
using UnityEngine;

public class Skill_AllRiversRunIntoSea : Skill
{
    private readonly Player player;
    private readonly RepeatTimer collectTimer;
    private readonly LayerMask propLayer;

    public Skill_AllRiversRunIntoSea(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        player = ServiceLocator.Get<Player>();
        collectTimer = new RepeatTimer();
        collectTimer.OnComplete += CollectAllExps;
        collectTimer.Initialize(skillInfoData.collectCD_AllRiversRunIntoSea, false);
        propLayer = LayerMask.GetMask("Prop");
    }

    protected override void OnUpgradeSkillGrade1()
    {
        player.CollectRadis += skillInfoData.collectRadiusIncrease1_AllRiversRunIntoSea;
    }

    protected override void OnRemoveSkillGrade1()
    {
        player.CollectRadis -= skillInfoData.collectRadiusIncrease1_AllRiversRunIntoSea;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        player.speed += skillInfoData.speedIncrease_AllRiversRunIntoSea;
    }

    protected override void OnRemoveSkillGrade2()
    {
        player.speed -= skillInfoData.speedIncrease_AllRiversRunIntoSea;
    }

    protected override void OnUpgradeSkillGrade3()
    {
        player.CollectRadis += skillInfoData.collectRadiusIncrease2_AllRiversRunIntoSea;
    }

    protected override void OnRemoveSkillGrade3()
    {
        player.CollectRadis -= skillInfoData.collectRadiusIncrease2_AllRiversRunIntoSea;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        collectTimer.Restart();
    }

    protected override void OnRemoveSkillGrade4()
    {
        collectTimer.Paused = true;
    }

    private void CollectAllExps()
    {
        Collider2D[] targetProps = Physics2D.OverlapCircleAll(player.transform.position,
            skillInfoData.absorbRadius_AllRiversRunIntoSea, propLayer);
        foreach (var targetProp in targetProps)
        {
            if (targetProp.TryGetComponent<EXPParticle>(out var exp) && exp.Collectable)
            {
                exp.FollowAndRecycle(player.transform);
            }
        }
    }
}
