using Services;
using System.Collections.Generic;
using UnityEngine;

public class Skill_PushOn : Skill
{
    private readonly ShadowManager shadowManager;
    private readonly Dictionary<Shadow, int> beatCounts;
    private readonly BeatInfo beatInfo;

    public Skill_PushOn(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        shadowManager = ServiceLocator.Get<ShadowManager>();
        beatCounts = new Dictionary<Shadow, int>();
        beatInfo = new BeatInfo(skillInfoData.beatDist_PushOn, 
            skillInfoData.beatDist_PushOn / skillInfoData.beatDuration_PushOn, true);
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowManager.OnSetShadow += SetShadowDamageOn;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowManager.OnSetShadow -= SetShadowDamageOn;
    }

    private void SetShadowDamageOn(Shadow shadow)
    {
        shadow.SetDamageOn(skillInfoData.damageMultiplier_PushOn);
    }

    protected override void OnUpgradeSkillGrade2()
    {
        eventSystem.AddListener<Shadow, Collider2D>(EEvent.OnShadowTrigger, TouchBeatEnemy);
        eventSystem.AddListener<Shadow>(EEvent.AfterCreateShadow, ClearBeatCount);
    }

    protected override void OnRemoveSkillGrade2()
    {
        eventSystem.RemoveListener<Shadow, Collider2D>(EEvent.OnShadowTrigger, TouchBeatEnemy);
        eventSystem.RemoveListener<Shadow>(EEvent.AfterCreateShadow, ClearBeatCount);
        beatCounts.Clear();
    }

    private void TouchBeatEnemy(Shadow shadow, Collider2D collision)
    {
        if (shadow.ShadowState == ShadowState.Setting && collision.CompareTag("Enemy"))
        {
            if (!beatCounts.ContainsKey(shadow))
            {
                beatCounts[shadow] = skillInfoData.beatCount_PushOn;
            }
            Enemy enemy = collision.GetComponentInParent<Enemy>();
            if (enemy != null && beatCounts[shadow]-- > 0)
            {
                beatInfo.beatVec = shadow.MoveDir.normalized * beatInfo.beatDist;
                enemy.GetBeaten(beatInfo);
            }
        }
    }

    private void ClearBeatCount(Shadow shadow)
    {
        beatCounts.Remove(shadow);
    }

    protected override void OnUpgradeSkillGrade3()
    {
        shadowManager.SetShadowWallSetable(true);
    }

    protected override void OnRemoveSkillGrade3()
    {
        shadowManager.SetShadowWallSetable(false);
    }

    protected override void OnUpgradeSkillGrade4()
    {
        shadowManager.shadowWallInfo.bidirectional = true;
    }

    protected override void OnRemoveSkillGrade4()
    {
        shadowManager.shadowWallInfo.bidirectional = false;
    }
}
