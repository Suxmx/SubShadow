using Services;

public class ShadowWaveLauncher : ShadowLauncher
{
    private int penetrateCount;

    public void Initialize(ShadowLauncherInfo shadowLauncherInfo, int penetrateCount)
    {
        Initialize(shadowLauncherInfo);
        this.penetrateCount = penetrateCount;
    }

    protected override void ShootToEnemy(Enemy enemy)
    {
        objectManager.Activate(EObject.ShadowWave, transform.position)
            .Transform.GetComponent<ShadowWave>().Initialize(
            enemy.transform.position - transform.position, shadowLauncherInfo, penetrateCount);
    }
}
