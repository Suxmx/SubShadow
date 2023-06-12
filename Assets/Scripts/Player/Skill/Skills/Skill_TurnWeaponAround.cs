using Services;
using UnityEngine;

public class Skill_TurnWeaponAround : Skill
{
    private readonly FanShapedInfo pushFanShapedInfo, chopFanShapedInfo;
    private readonly ShadowManager shadowManager;
    private readonly ObjectManager objectManager;
    private readonly Player player;
    private readonly AudioManager audioManager;
    private float flyingSpeedDamageMultiplier;

    public Skill_TurnWeaponAround(SkillInfo skillInfo, SkillInfoData skillInfoData, EventSystem eventSystem) 
        : base(skillInfo, skillInfoData, eventSystem, true)
    {
        BeatInfo beatInfo = new BeatInfo(skillInfoData.pushBeatDist_TurnWeaponAround,
                skillInfoData.pushBeatDist_TurnWeaponAround / skillInfoData.pushBeatDuration_TurnWeaponAround, true);
        pushFanShapedInfo = new FanShapedInfo(skillInfoData.pushRadius_TurnWeaponAround, 
            skillInfoData.pushCentralAngle_TurnWeaponAround, beatInfo);
        chopFanShapedInfo = new FanShapedInfo(skillInfoData.chopRadius_TurnWeaponAround, 
            skillInfoData.chopCentralAngle_TurnWeaponAround, beatInfo);
        shadowManager = ServiceLocator.Get<ShadowManager>();
        objectManager = ServiceLocator.Get<ObjectManager>();
        audioManager=ServiceLocator.Get<AudioManager>();
        player = ServiceLocator.Get<Player>();
        flyingSpeedDamageMultiplier = 0f;
    }

    protected override void OnUpgradeSkillGrade1()
    {
        shadowManager.OnSetShadow += GeneratePushFanShapedExplode;
    }

    protected override void OnRemoveSkillGrade1()
    {
        shadowManager.OnSetShadow -= GeneratePushFanShapedExplode;
    }

    private void GeneratePushFanShapedExplode(Shadow shadow)
    {
        objectManager.Activate(EObject.EnergyPulse140, player.transform.position, player.transform)
            .Transform.GetComponent<FanShapedExplodeTrigger>().Initialize(pushFanShapedInfo, shadow.MoveDir);
    }

    protected override void OnUpgradeSkillGrade2()
    {
        shadowManager.AfterRecallShadow += GenerateChopFanShapedExplode;
    }

    protected override void OnRemoveSkillGrade2()
    {
        shadowManager.AfterRecallShadow -= GenerateChopFanShapedExplode;
    }

    private void GenerateChopFanShapedExplode(Shadow shadow)
    {
        Vector3 cursorWorldPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 playerToCursor = cursorWorldPos - player.transform.position;

        float damage = shadowManager.shadowInfo.Damage * skillInfoData.chopDamageMultiplier_TurnWeaponAround 
            + flyingSpeedDamageMultiplier * Mathf.Pow(shadowManager.shadowInfo.flyingSpeed, 3);
        objectManager.Activate(ownedGrade >= 4? EObject.EnergyPulse210 : EObject.EnergyPulse70, 
            player.transform.position, player.transform).Transform
            .GetComponent<FanShapedExplodeTrigger>().Initialize(chopFanShapedInfo, 
            playerToCursor.sqrMagnitude > 0 ? playerToCursor.normalized : shadow.MoveDir, damage);
        audioManager.PlaySound("TurnWeaponAround_Pulse",AudioPlayMode.Plenty);
    }

    protected override void OnUpgradeSkillGrade3()
    {
        flyingSpeedDamageMultiplier = skillInfoData.flyingSpeedDamageMultiplier_TurnWeaponAround;
    }

    protected override void OnRemoveSkillGrade3()
    {
        flyingSpeedDamageMultiplier = 0f;
    }

    protected override void OnUpgradeSkillGrade4()
    {
        chopFanShapedInfo.fanShapedAngle = skillInfoData.expandChopCentralAngle_TurnWeaponAround;
        chopFanShapedInfo.fanShapedRadius = skillInfoData.expandChopRadius_TurnWeaponAround;
    }

    protected override void OnRemoveSkillGrade4()
    {
        chopFanShapedInfo.fanShapedAngle = skillInfoData.chopCentralAngle_TurnWeaponAround;
        chopFanShapedInfo.fanShapedRadius = skillInfoData.chopRadius_TurnWeaponAround;
    }
}
