[System.Serializable]
public class EnemyInfo
{
    [Label("�������")]
    public EnemyType enemyType;
    [Label("����Ѫ��")]
    public float HP;
    [Label("����׷������")]
    public float followSpeed;
    [Label("���˾������")]
    public int dropEXP = 1;
    [Label("�Ǿ�Ӣ����")]
    public bool isBoss;
}
