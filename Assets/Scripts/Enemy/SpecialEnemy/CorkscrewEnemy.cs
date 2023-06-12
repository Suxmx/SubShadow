using UnityEngine;

public class CorkscrewEnemy : Enemy
{
    private CorkscrewEnemyGroupInfo corkscrewEnemyGroupInfo;
    private Vector3 centerPos, dirToCenter, dirToTangent;
    private float distToCenter;

    public void Initialize(SpecialEnemyInfo enemyInfo, EnemyGroupTracker enemyGroupTracker,
        CorkscrewEnemyGroupInfo corkscrewEnemyGroupInfo, Vector3 centerPos)
    {
        enemyStatusInfo.Initialize(enemyInfo.HP, false, enemyInfo.dropEXP *
            (enemyGroupTracker == null ? 1 : enemyGroupTracker.expMultiplier));
        attachGroupTracker = enemyGroupTracker;
        this.corkscrewEnemyGroupInfo = corkscrewEnemyGroupInfo;
        this.centerPos = centerPos;
        followSpeed = corkscrewEnemyGroupInfo.radialSpeed;
        GainControl();
        ArmSelf();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        faceTool.SetFace(dirToCenter.x);
    }

    protected override void EnemyFixedUpdate()
    {
        distToCenter = Vector3.Distance(centerPos, transform.position);
        if (distToCenter < corkscrewEnemyGroupInfo.finalDist)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        dirToCenter = (centerPos - transform.position) / distToCenter;
        if (corkscrewEnemyGroupInfo.clockwise) dirToTangent = new Vector3(dirToCenter.y, -dirToCenter.x);
        else dirToTangent = new Vector3(-dirToCenter.y, dirToCenter.x);
        rb.velocity = enemySpeedMultiplier.multiplier * (speed * dirToCenter + 
            corkscrewEnemyGroupInfo.angularSpeed * Mathf.Deg2Rad * distToCenter * dirToTangent);
    }

    protected override void GainControl()
    {
        base.GainControl();
        speedTimer.Initialize(0f, followSpeed, 1f);
    }
}
