[System.Serializable]
public class ShadowInfo
{
    [Label("��������")]
    public float basicDamage;
    [Label("������������")]
    public float basicDamageMultiplier;
    public float Damage => basicDamage * basicDamageMultiplier;
    [Label("�������")]
    public float basicScale;
    [Label("����ʱ��")]
    public float stayingDuration;
    [Label("������")]
    public int missileCount;
    [Label("�����ٶ�")]
    public float flyingSpeed;
    [Label("���о���ת���������")]
    public float flyingDistScaleMultiplier;
    [Label("�������")]
    public float scaleUpperLimit;
    [Label("��������")]
    public float missileScale;
    [Label("�ջ�����ϵ��")]
    public float recallDamageMultiplier;
    [Label("�������ϵ��")]
    public float scaleDamageMultiplier;
    [Label("������о���")]
    public float reflectFlyingDist;
    [Label("�ȼ�����")]
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
