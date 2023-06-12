[System.Serializable]
public class ShadowWallInfo
{
    [Label("威力乘数")]
    public float damageMultiplier;
    [Label("特殊发射物威力乘数")]
    public float missileDamageMultiplier;
    [Label("滞留时长")]
    public float stayingDuration;
    [Label("拼接数")]
    public int missileJoinCount;
    [Label("拼接距离")]
    public float missileJoinDist;
    [Label("双向发射")]
    public bool bidirectional;

    public ShadowWallInfo(SkillInfoData skillInfoData)
    {
        damageMultiplier = skillInfoData.damageMultiplier_PushOn;
        missileDamageMultiplier = skillInfoData.missileDamageMultiplier_PushOn;
        stayingDuration = skillInfoData.missileStayingDuration_PushOn;
        missileJoinCount = skillInfoData.missileJoinCount_PushOn;
        missileJoinDist = skillInfoData.missileJoinDist_PushOn;
        bidirectional = false;
    }
}
