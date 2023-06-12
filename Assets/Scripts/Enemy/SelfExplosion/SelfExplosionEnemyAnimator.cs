public class SelfExplosionEnemyAnimator : EnemyAnimator
{
    public void PlayFlickMotion() => animator.Play("Flick");
    public override void PlayDestroySelfMotion()
    {
        base.PlayDestroySelfMotion();
        audioManager.PlaySound("SelfExplosion_Explode",AudioPlayMode.Plenty);
    }
}
