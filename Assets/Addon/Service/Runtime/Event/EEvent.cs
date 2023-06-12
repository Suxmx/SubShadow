namespace Services
{
    public enum EEvent
    {
        /// <summary>
        /// 加载场景前，参数：即将加载的场景号
        /// </summary>
        BeforeLoadScene,
        /// <summary>
        /// 加载场景后（至少一帧以后），参数：刚加载好的场景号
        /// </summary>
        AfterLoadScene,
        /// <summary>
        /// 升级技能时，参数：技能
        /// </summary>
        OnUpgradeSkill,
        /// <summary>
        /// 移除技能时，参数：技能
        /// </summary>
        OnRemoveSkill,
        /// <summary>
        /// 影子创造完毕时（停下），参数：影子
        /// </summary>
        AfterCreateShadow,
        /// <summary>
        /// 影子触发器触发时，参数：影子，另一个碰撞器
        /// </summary>
        OnShadowTrigger,
        /// <summary>
        /// 影子回收之前，参数：影子
        /// </summary>
        BeforeRecallShadow,
        /// <summary>
        /// 伤害敌人时，参数：敌人、造成伤害的IDamager
        /// </summary>
        OnHurtEnemy,
        /// <summary>
        /// 杀死敌人时
        /// </summary>
        OnKillEnemy,
    }
}