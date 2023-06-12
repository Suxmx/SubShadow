using Services;
using UnityEngine;

public class ShadowArrowLauncher : ShadowLauncher
{
    private ShadowLastingTimer lastingTimer;
    private float arrowIntervalUpperLimit, arrowIntervalLowerLimit;
    private AudioManager audioManager;
    protected override void Awake()
    {
        base.Awake();
        audioManager=ServiceLocator.Get<AudioManager>();
    }

    public void Initialize(ShadowLauncherInfo shadowLauncherInfo, float arrowIntervalUpperLimit, 
        float arrowIntervalLowerLimit, ShadowLastingTimer lastingTimer)
    {
        this.shadowLauncherInfo = shadowLauncherInfo;
        this.lastingTimer = lastingTimer;
        this.arrowIntervalUpperLimit = arrowIntervalUpperLimit;
        this.arrowIntervalLowerLimit = arrowIntervalLowerLimit;
        shootTimer.Initialize(arrowIntervalUpperLimit);
        shootTimer.OnComplete += DecreaseInterval;
        myObject.OnRecycle += ResetEvent;
    }

    private void DecreaseInterval()
    {
        shootTimer.SetInterval(Mathf.Max(arrowIntervalUpperLimit - lastingTimer.Time, arrowIntervalLowerLimit));
    }

    private void ResetEvent()
    {
        shootTimer.OnComplete -= DecreaseInterval;
        myObject.OnRecycle -= ResetEvent;
    }

    protected override void ShootToEnemy(Enemy enemy)
    {
        objectManager.Activate(EObject.ShadowArrow, transform.position)
            .Transform.GetComponent<ShadowArrow>().Initialize(
            enemy.transform.position - transform.position, shadowLauncherInfo);
        audioManager.PlaySound("FlyingArrow_Missile",AudioPlayMode.Plenty);//此处有待商榷
    }
}
