public class ShadowLauncherInfo
{
    /// <summary>
    /// �˺�
    /// </summary>
    public float attackDamage;
    /// <summary>
    /// �������
    /// </summary>
    public float attackInterval;
    /// <summary>
    /// ̽�����
    /// </summary>
    public float attackDist;
    /// <summary>
    /// �ӵ��ٶ�
    /// </summary>
    public float bulletSpeed;
    /// <summary>
    /// ������
    /// </summary>
    public float maxFlyingDist;
    /// <summary>
    /// ������
    /// </summary>
    public int missileCount;
    /// <summary>
    /// ���������
    /// </summary>
    public float missileScale;

    public ShadowLauncherInfo(float attackDamage, float attackInterval, float attackDist, 
        float bulletSpeed, float maxFlyingDist)
    {
        this.attackDamage = attackDamage;
        this.attackInterval = attackInterval;
        this.attackDist = attackDist;
        this.bulletSpeed = bulletSpeed;
        this.maxFlyingDist = maxFlyingDist;
    }
}
