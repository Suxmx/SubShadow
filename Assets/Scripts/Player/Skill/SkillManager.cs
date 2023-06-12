using Services;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10)]
public class SkillManager : Service
{
    [Other] private EventSystem eventSystem;

    public Dictionary<SkillType, Skill> attackSkills;
    public Dictionary<SkillType, Skill> assistantSkills;
    public HashSet<Skill> allowedSkills;
    private int attackSkillCount, assistantSkillCount;

    public SkillInfoData SkillInfoData { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        SkillInfoData = Resources.Load<SkillInfoData>("SkillInfoData");
    }

    protected override void Start()
    {
        base.Start();
        InitializeSkills();

        eventSystem.AddListener<Skill>(EEvent.OnUpgradeSkill, OnUpgradeSkill);
        eventSystem.AddListener<Skill>(EEvent.OnRemoveSkill, OnRemoveSkill);
        eventSystem.AddListener<int>(EEvent.BeforeLoadScene, BeforeLoadScene);
        eventSystem.AddListener<int>(EEvent.AfterLoadScene, AfterLoadScene);
    }

    /// <summary>
    /// 获取可用的技能，用于加点界面
    /// </summary>
    /// <param name="count">最多获取的个数</param>
    public List<Skill> GetUsableSkills(int count = 3) => allowedSkills.RandomPick(count);

    public void UpgradeAllSkills()
    {
        foreach (var skill in attackSkills.Values)
        {
            while (skill.ownedGrade < 4) skill.UpgradeSkill();
        }
        foreach (var skill in assistantSkills.Values)
        {
            while (skill.ownedGrade < 4) skill.UpgradeSkill();
        }
    }

    public void RemoveAllSkills()
    {
        foreach (var skill in attackSkills.Values)
        {
            skill.RemoveSkill();
        }
        foreach (var skill in assistantSkills.Values)
        {
            skill.RemoveSkill();
        }
    }

    private void OnUpgradeSkill(Skill skill)
    {
        if (skill.ownedGrade == 1)
        {
            if ((skill.isAttackSkill && ++attackSkillCount >= 4) || 
                (!skill.isAttackSkill && ++assistantSkillCount >= 4))
            {
                List<Skill> removeList = new List<Skill>();
                foreach (var aSkill in allowedSkills)
                {
                    if (aSkill.isAttackSkill == skill.isAttackSkill && aSkill.ownedGrade == 0)
                    {
                        removeList.Add(aSkill);
                    }
                }
                foreach (var rSkill in removeList)
                {
                    allowedSkills.Remove(rSkill);
                }
            }
        }
        else if (skill.ownedGrade == 4 && !skill.alwaysUpgradable)
        {
            allowedSkills.Remove(skill);
        }
    }

    private void OnRemoveSkill(Skill skill)
    {
        foreach (var aSkill in (skill.isAttackSkill ? attackSkills : assistantSkills).Values)
        {
            if (skill.ownedGrade < 4 || skill.alwaysUpgradable)
                allowedSkills.Add(aSkill);
        }
    }

    private void BeforeLoadScene(int _)
    {
        RemoveAllSkills();
        attackSkillCount = assistantSkillCount = 0;
    }

    private void AfterLoadScene(int _)
    {
        if (SceneController.InGame)
        {
            // 暂时的测试技能用
            //ServiceLocator.Get<GlobalGameCycle>().AttachToGameCycle(EInvokeMode.NextUpdate, Test);
        }
    }

    // 测试技能
    //private void Test()
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        UpgradeSkill(SkillType.ShadowShinFoot);
    //        UpgradeSkill(SkillType.ShadowDisappearInstantly);
    //        UpgradeSkill(SkillType.Berserker);
    //        UpgradeSkill(SkillType.ShadowFollower);
    //        UpgradeSkill(SkillType.Vortex);
    //    }
    //}

    private void InitializeSkills()
    {
        Dictionary<SkillType, SkillInfo> attackSkillInfos = new Dictionary<SkillType, SkillInfo>();
        Dictionary<SkillType, SkillInfo> assistantSkillInfos = new Dictionary<SkillType, SkillInfo>();
        SkillInfoData.attackSkillInfos.ForEach(x => attackSkillInfos.Add(x.skillType, x));
        SkillInfoData.assistantSkillInfos.ForEach(x => assistantSkillInfos.Add(x.skillType, x));
        attackSkills = new Dictionary<SkillType, Skill>
        {
            { SkillType.Whirl, new Skill_Whirl(
                attackSkillInfos[SkillType.Whirl], SkillInfoData, eventSystem) },
            { SkillType.Reap, new Skill_Reap(
                attackSkillInfos[SkillType.Reap], SkillInfoData, eventSystem) },
            { SkillType.Sunspot, new Skill_Sunspot(
                attackSkillInfos[SkillType.Sunspot], SkillInfoData, eventSystem) },
            { SkillType.TurnWeaponAround, new Skill_TurnWeaponAround(
                attackSkillInfos[SkillType.TurnWeaponAround], SkillInfoData, eventSystem) },
            { SkillType.NightmarishFlame, new Skill_NightmarishFlame(
                attackSkillInfos[SkillType.NightmarishFlame], SkillInfoData, eventSystem) },
            { SkillType.FlyingArrow, new Skill_FlyingArrow(
                attackSkillInfos[SkillType.FlyingArrow], SkillInfoData, eventSystem) },
            { SkillType.RoughSea, new Skill_RoughSea(
                attackSkillInfos[SkillType.RoughSea], SkillInfoData, eventSystem) },
            { SkillType.PushOn, new Skill_PushOn(
                attackSkillInfos[SkillType.PushOn], SkillInfoData, eventSystem) }
        };
        assistantSkills = new Dictionary<SkillType, Skill>
        {
            { SkillType.Gather, new Skill_Gather(
                assistantSkillInfos[SkillType.Gather], SkillInfoData, eventSystem) },
            { SkillType.ConfusedNoise, new Skill_ConfusedNoise(
                assistantSkillInfos[SkillType.ConfusedNoise], SkillInfoData, eventSystem) },
            { SkillType.RealImage, new Skill_RealImage(
                assistantSkillInfos[SkillType.RealImage], SkillInfoData, eventSystem) },
            { SkillType.Ghost, new Skill_Ghost(
                assistantSkillInfos[SkillType.Ghost], SkillInfoData, eventSystem) },
            { SkillType.Unbridled, new Skill_Unbridled(
                assistantSkillInfos[SkillType.Unbridled], SkillInfoData, eventSystem) },
            { SkillType.Clone, new Skill_Clone(
                assistantSkillInfos[SkillType.Clone], SkillInfoData, eventSystem) },
            { SkillType.Reborn, new Skill_Reborn(
                assistantSkillInfos[SkillType.Reborn], SkillInfoData, eventSystem) },
            { SkillType.AllRiversRunIntoSea, new Skill_AllRiversRunIntoSea(
                assistantSkillInfos[SkillType.AllRiversRunIntoSea], SkillInfoData, eventSystem) },
        };
        allowedSkills = new HashSet<Skill>();
        foreach (var skill in attackSkills.Values) allowedSkills.Add(skill);
        foreach (var skill in assistantSkills.Values) allowedSkills.Add(skill);
    }
}
