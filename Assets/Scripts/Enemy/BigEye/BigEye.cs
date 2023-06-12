using MyTimer;
using Services;
using UnityEngine;

public enum BigEyeState
{
    Chase,
    Idle,
    Flee,
}

public class BigEye : Enemy
{
    private ObjectManager objectManager;

    private BigEyeInfo bigEyeInfo;
    private float distToPlayer;
    private RepeatTimer shootTimer;
    private CountdownTimer whileShootingTimer;
    private bool IsShooting => !whileShootingTimer.Paused;

    private BigEyeState currentState;
    public BigEyeState CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState != value)
            {
                currentState = value;
                SetSpeedTimer();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        objectManager = ServiceLocator.Get<ObjectManager>();
        bigEyeInfo = ServiceLocator.Get<EnemyManager>().EnemyInfoData.bigEyeInfo;

        shootTimer = new RepeatTimer();
        shootTimer.Initialize(bigEyeInfo.shootInterval, false);
        shootTimer.OnComplete += Shoot;

        whileShootingTimer = new CountdownTimer();
        whileShootingTimer.Initialize(bigEyeInfo.stillDurationWhileShooting, false);
        whileShootingTimer.OnComplete += SetSpeedTimer;
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        currentState = BigEyeState.Chase;
    }

    protected override void EnemyFixedUpdate()
    {
        if (!IsShooting)
        {
            ChangeState();
            MoveToPlayer();
        }
    }

    private void Shoot()
    {
        rb.velocity = Vector2.zero;
        ResetSpeedTimer();

        whileShootingTimer.Restart();
        objectManager.Activate(EObject.EnemyBullet, transform.position)
            .Transform.GetComponent<NormalBullet>().Initialize(
            playerTransform.position - transform.position, bigEyeInfo.bulletDamage, 
            bigEyeInfo.bulletSpeed, bigEyeInfo.maxFlyingDist);
    }

    // 在FixedUpdate中检测是否需要切换当前的状态
    private void ChangeState()
    {
        distToPlayer = Vector3.Distance(playerTransform.position, transform.position);
        if (distToPlayer > bigEyeInfo.keepDist + bigEyeInfo.stillDist)
        {
            CurrentState = BigEyeState.Chase;
            shootTimer.Paused = true;
        }
        else
        {
            if (distToPlayer < bigEyeInfo.keepDist - bigEyeInfo.stillDist)
            {
                CurrentState = BigEyeState.Flee;
            }
            else
            {
                CurrentState = BigEyeState.Idle;
            }
            if (disarmLock.Unlocked) shootTimer.Paused = false;
        }
    }

    private void SetSpeedTimer()
    {
        switch (CurrentState)
        {
            case BigEyeState.Chase:
                speedTimer.Initialize(speed, followSpeed, 1f);
                break;
            case BigEyeState.Idle:
                speedTimer.Initialize(speed, 0f, 1f);
                break;
            case BigEyeState.Flee:
                speedTimer.Initialize(speed, -followSpeed, 1f);
                break;
        }
    }

    protected override void GainControl()
    {
        base.GainControl();
        SetSpeedTimer();
    }

    protected override void DisarmSelf()
    {
        base.DisarmSelf();
        shootTimer.Paused = true;
    }

    protected override void ArmSelf()
    {
        base.ArmSelf();
        shootTimer.Paused = false;
    }

    protected override void OnDestroySelf()
    {
        base.OnDestroySelf();
        shootTimer.ReturnToZero();
        whileShootingTimer.Paused = true;
    }
}
