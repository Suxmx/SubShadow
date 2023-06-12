namespace Services
{
    public enum EEvent
    {
        /// <summary>
        /// ���س���ǰ���������������صĳ�����
        /// </summary>
        BeforeLoadScene,
        /// <summary>
        /// ���س���������һ֡�Ժ󣩣��������ռ��غõĳ�����
        /// </summary>
        AfterLoadScene,
        /// <summary>
        /// ��������ʱ������������
        /// </summary>
        OnUpgradeSkill,
        /// <summary>
        /// �Ƴ�����ʱ������������
        /// </summary>
        OnRemoveSkill,
        /// <summary>
        /// Ӱ�Ӵ������ʱ��ͣ�£���������Ӱ��
        /// </summary>
        AfterCreateShadow,
        /// <summary>
        /// Ӱ�Ӵ���������ʱ��������Ӱ�ӣ���һ����ײ��
        /// </summary>
        OnShadowTrigger,
        /// <summary>
        /// Ӱ�ӻ���֮ǰ��������Ӱ��
        /// </summary>
        BeforeRecallShadow,
        /// <summary>
        /// �˺�����ʱ�����������ˡ�����˺���IDamager
        /// </summary>
        OnHurtEnemy,
        /// <summary>
        /// ɱ������ʱ
        /// </summary>
        OnKillEnemy,
    }
}