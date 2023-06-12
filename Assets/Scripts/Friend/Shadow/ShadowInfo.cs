[System.Serializable]
public class ShadowInfo
{
    [Label("基础威力")]
    public float basicDamage;
    [Label("基础威力乘数")]
    public float basicDamageMultiplier;
    public float Damage => basicDamage * basicDamageMultiplier;
    [Label("基础规格")]
    public float basicScale;
    [Label("滞留时长")]
    public float stayingDuration;
    [Label("发射量")]
    public int missileCount;
    [Label("飞行速度")]
    public float flyingSpeed;
    [Label("飞行距离转对象规格乘数")]
    public float flyingDistScaleMultiplier;
    [Label("规格上限")]
    public float scaleUpperLimit;
    [Label("发射物规格")]
    public float missileScale;
    [Label("收回威力系数")]
    public float recallDamageMultiplier;
    [Label("规格威力系数")]
    public float scaleDamageMultiplier;
    [Label("反射飞行距离")]
    public float reflectFlyingDist;
    [Label("等级乘数")]
    public float gradeMultiplier;

    public void Initialize(SkillInfoData skillInfoData)
    {
        basicDamage = skillInfoData.basicDamage;
        basicDamageMultiplier = 1f;
        basicScale = skillInfoData.basicShadowScale;
        stayingDuration = skillInfoData.shadowStayingDuration;
        missileCount = skillInfoData.missileCount;
        flyingSpeed = skillInfoData.flyingSpeed;
        flyingDistScaleMultiplier = 0f;
        scaleUpperLimit = skillInfoData.scaleUpperLimit_Unbridled;
        missileScale = skillInfoData.missileScale;
        recallDamageMultiplier = 1f;
        scaleDamageMultiplier = 0f;
        reflectFlyingDist = 0f;
        gradeMultiplier = 0f;
    }
}
