[System.Serializable]
public class CorkscrewEnemyGroupInfo
{
    [Label("�����������")]
    public int index;
    [Label("��Ⱥ�ܽǶ�")]
    public float totalAngle;
    [Label("��Ⱥ��ʼ����")]
    public float originalDist;
    [Label("��Ⱥ���վ���")]
    public float finalDist;
    [Label("��ȺȦ���")]
    public float circleSpacing;
    [Label("��Ⱥ�����ٶ�")]
    public float radialSpeed;
    [Label("��Ⱥ���ٶ�")]
    public float angularSpeed;
    [Label("˳ʱ����ת")]
    public bool clockwise;
    [Label("��Ⱥ��������")]
    public float spawnInterval;
    [Label("��Ⱥ���鱶��")]
    public int expMultiplier = 1;
}
