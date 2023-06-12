using MyTimer;
using Services;

public class Skill_Reborn : Skill
{
    private readonly PlayerStatusInfo playerStatusInfo;
    private readonly RepeatTimer healTimer;
    private readonly AudioManager audioManager;
    private int killCount;

    public Skill_Reborn(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, false)
    {
        playerStatusInfo = ServiceLocator.Get<Player>().PlayerStatusInfo;
        healTimer = new RepeatTimer();
        healTimer.OnComplete += GetHealed;
        healTimer.Initialize(skillInfoData.healDuration_Reborn, false);
        audioManager=ServiceLocator.Get<AudioManager>();
    }

    protected override void OnUpgradeSkillGrade1()
    {
        playerStatusInfo.MaxHP += skillInfoData.maxHPIncrease_Reborn;
        playerStatusInfo.CurrentHP += skillInfoData.maxHPIncrease_Reborn;
    }

    protected override void OnRemoveSkillGrade1()
    {
        playerStatusInfo.MaxHP -= skillInfoData.maxHPIncrease_Reborn;
    }

    protected override void OnUpgradeSkillGrade2()
    {
        healTimer.Restart();
    }

    protected override void OnRemoveSkillGrade2()
    {
        healTimer.Paused = true;
    }

    private void GetHealed()
    {
        if (playerStatusInfo.CurrentHP == playerStatusInfo.MaxHP)
        {
            playerStatusInfo.MaxHP++;
        }
        playerStatusInfo.GetHealed(1);
    }

    protected override void OnUpgradeSkillGrade3()
    {
        killCount = 0;
        eventSystem.AddListener(EEvent.OnKillEnemy, HealOnKillEnemy);
    }

    protected override void OnRemoveSkillGrade3()
    {
        eventSystem.RemoveListener(EEvent.OnKillEnemy, HealOnKillEnemy);
    }

    private void HealOnKillEnemy()
    {
        if (++killCount == skillInfoData.healKillCount_Reborn)
        {
            playerStatusInfo.GetHealed(1);
            killCount = 0;
        }
    }

    protected override void OnUpgradeSkillGrade4()
    {
        playerStatusInfo.OnGetHurt += BottleSpirit;
    }

    protected override void OnRemoveSkillGrade4()
    {
        playerStatusInfo.OnGetHurt -= BottleSpirit;
    }

    private void BottleSpirit()
    {
        if (!playerStatusInfo.IsAlive)
        {
            audioManager.PlaySound("Reborn_Reborn",AudioPlayMode.Wait);
            playerStatusInfo.GetHealed(playerStatusInfo.MaxHP);
            playerStatusInfo.OnGetHurt -= BottleSpirit;
        }
    }
}
