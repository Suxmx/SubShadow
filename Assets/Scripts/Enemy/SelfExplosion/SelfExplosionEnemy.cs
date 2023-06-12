using MyTimer;
using Services;
using UnityEngine;

public class SelfExplosionEnemy : Enemy
{
    private SelfExplosionDamager explosionDamager;
    private CountdownTimer explodeTimer;
    private float explodeSqrDist;

    protected override void Awake()
    {
        base.Awake();

        SelfExplosionEnemyInfo selfExplosionEnemyInfo = 
            ServiceLocator.Get<EnemyManager>().EnemyInfoData.selfExplosionEnemyInfo;
        explosionDamager = GetComponentInChildren<SelfExplosionDamager>();
        explosionDamager.Init(selfExplosionEnemyInfo);
        explosionDamager.Active = false;
        transform.Find("DetectRadius").localScale = new Vector3(
            2 * selfExplosionEnemyInfo.explodeDist, 2 * selfExplosionEnemyInfo.explodeDist, 1f);
        explodeTimer = new CountdownTimer();
        explodeTimer.Initialize(selfExplosionEnemyInfo.explosionDuration, false);
        explodeTimer.OnComplete += DestroySelf;
        explodeSqrDist = Mathf.Pow(selfExplosionEnemyInfo.explodeDist, 2);
    }

    protected override void EnemyFixedUpdate()
    {
        MoveToPlayer();
        if (explodeTimer.Paused && DetectPlayerDist())
        {
            (enemyAnimator as SelfExplosionEnemyAnimator).PlayFlickMotion();
            explodeTimer.Restart();
            audioManager.PlaySound("SelfExplosion_Preexplode",AudioPlayMode.Plenty);
        }
    }

    protected override void GainControl()
    {
        base.GainControl();
        speedTimer.Initialize(0f, followSpeed, 1f);
    }

    private bool DetectPlayerDist()
    {
        return Vector2.SqrMagnitude(transform.position - playerTransform.position) < explodeSqrDist;
    }

    protected override void OnDestroySelf()
    {
        base.OnDestroySelf();
        explodeTimer.Paused = true;
        enemyStatusInfo.CurrentHP = 0;
        explosionDamager.Active = true;
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
        explodeTimer.Paused = true;
        explosionDamager.ResetDamager();
    }
}
