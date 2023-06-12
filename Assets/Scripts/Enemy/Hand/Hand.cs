public class Hand : Enemy
{
    protected override void EnemyFixedUpdate()
    {
        MoveToPlayer();
    }

    protected override void GainControl()
    {
        base.GainControl();
        speedTimer.Initialize(0f, followSpeed, 1f);
    }
}
