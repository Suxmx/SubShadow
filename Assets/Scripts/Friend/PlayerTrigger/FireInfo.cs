public class FireInfo
{
    /// <summary>
    /// ȼ�ճ�����ʱ��
    /// </summary>
    public float fireDuration;
    /// <summary>
    /// ȼ���ж����
    /// </summary>
    public float judgeInterval;
    /// <summary>
    /// ȼ�յ����ж��˺�
    /// </summary>
    public float damage;
    /// <summary>
    /// ��������ת�����˺�����
    /// </summary>
    public float hpDamageMulitiplier;
    public PlayerStatusInfo playerStatusInfo;
    public float HPDamageMultiplier => hpDamageMulitiplier * playerStatusInfo.MaxHP;
}
