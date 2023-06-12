[System.Serializable]
public class ShadowWallInfo
{
    [Label("��������")]
    public float damageMultiplier;
    [Label("���ⷢ������������")]
    public float missileDamageMultiplier;
    [Label("����ʱ��")]
    public float stayingDuration;
    [Label("ƴ����")]
    public int missileJoinCount;
    [Label("ƴ�Ӿ���")]
    public float missileJoinDist;
    [Label("˫����")]
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
