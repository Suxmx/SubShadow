using Services;

public abstract class Skill
{
    /// <summary>
    /// 技能信息
    /// </summary>
    public SkillInfo skillInfo;

    /// <summary>
    /// 已拥有的等级
    /// </summary>
    public int ownedGrade;

    /// <summary>
    /// 是否是攻击技能
    /// </summary>
    public bool isAttackSkill;

    /// <summary>
    /// 是否可一直升级
    /// </summary>
    public bool alwaysUpgradable;

    protected readonly SkillInfoData skillInfoData;
    protected readonly EventSystem eventSystem;

    public Skill(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem, 
        bool attackSkill, bool alwaysUpgradable = false)
    {
        this.skillInfo = skillInfo;
        this.skillInfoData = skillInfoData;
        this.eventSystem = eventSystem;
        this.isAttackSkill = attackSkill;
        this.alwaysUpgradable = alwaysUpgradable;
        ownedGrade = 0;
    }

    public virtual void UpgradeSkill()
    {
        ownedGrade++;
        switch (ownedGrade)
        {
            case 1:
                OnUpgradeSkillGrade1();
                break;
            case 2:
                OnUpgradeSkillGrade2();
                break;
            case 3:
                OnUpgradeSkillGrade3();
                break;
            case 4:
                OnUpgradeSkillGrade4();
                break;
            default:
                if (alwaysUpgradable)
                    OnUpgradeSkillGrade4();
                break;
        }
        eventSystem.Invoke(EEvent.OnUpgradeSkill, this);
    }

    public virtual void RemoveSkill()
    {
        if (ownedGrade >= 4)
            OnRemoveSkillGrade4();
        if (ownedGrade >= 3)
            OnRemoveSkillGrade3();
        if (ownedGrade >= 2)
            OnRemoveSkillGrade2();
        if (ownedGrade >= 1)
        {
            OnRemoveSkillGrade1();
            ownedGrade = 0;
            eventSystem.Invoke(EEvent.OnRemoveSkill, this);
        }
    }

    protected abstract void OnUpgradeSkillGrade1();
    protected abstract void OnUpgradeSkillGrade2();
    protected abstract void OnUpgradeSkillGrade3();
    protected abstract void OnUpgradeSkillGrade4();
    protected abstract void OnRemoveSkillGrade1();
    protected abstract void OnRemoveSkillGrade2();
    protected abstract void OnRemoveSkillGrade3();
    protected abstract void OnRemoveSkillGrade4();
}
