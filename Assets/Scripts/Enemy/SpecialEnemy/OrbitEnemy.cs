using UnityEngine;

public class OrbitEnemy : Enemy
{
    private Vector2 moveDir;

    public void Initialize(SpecialEnemyInfo enemyInfo, EnemyGroupTracker enemyGroupTracker, 
        float moveSpeed, Vector2 moveDir)
    {
        enemyStatusInfo.Initialize(enemyInfo.HP, false, enemyInfo.dropEXP *
            (enemyGroupTracker == null ? 1 : enemyGroupTracker.expMultiplier));
        attachGroupTracker = enemyGroupTracker;
        followSpeed = moveSpeed;
        this.moveDir = moveDir.normalized;
        faceTool.SetFace(moveDir.x);
        GainControl();
        ArmSelf();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        faceTool.SetFace(moveDir.x);
    }

    protected override void EnemyFixedUpdate()
    {
        rb.velocity = enemySpeedMultiplier.multiplier * speed * moveDir;
    }

    protected override void GainControl()
    {
        base.GainControl();
        speedTimer.Initialize(0f, followSpeed, 1f);
    }
}
