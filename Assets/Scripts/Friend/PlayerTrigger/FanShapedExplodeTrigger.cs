using MyTimer;
using UnityEngine;

public class FanShapedExplodeTrigger : ExplodeTrigger
{
    private FanShapedInfo fanShapedInfo;
    private CustomizedVFX.VFXEnergyPluse vfx;
    private CountdownTimer damageTrigger;

    protected override void Awake()
    {
        base.Awake();
        vfx = GetComponent<CustomizedVFX.VFXEnergyPluse>();

        destroyTimer.Initialize(0.5f, false);
        damageTrigger = new CountdownTimer();
        damageTrigger.OnComplete += () => cd.enabled = false;
        damageTrigger.Initialize(0.1f, false);
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        cd.enabled = true;
        damageTrigger.Restart();
    }

    protected override void OnRecycle()
    {
        base.OnRecycle();
        damageTrigger.Paused = true;
    }

    public void Initialize(FanShapedInfo fanShapedInfo, Vector3 fanShapedDir, float damage = 0f)
    {
        this.fanShapedInfo = fanShapedInfo;
        transform.up = fanShapedDir;
        vfx.up = fanShapedDir;
        Initialize(damage, fanShapedInfo.fanShapedRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponentInParent<Enemy>();
        if (enemy != null && enemy.IsAlive)
        {
            fanShapedInfo.beatInfo.beatVec = (collision.transform.position - transform.position)
                .normalized * fanShapedInfo.beatInfo.beatDist;
            enemy.GetBeaten(fanShapedInfo.beatInfo);
        }
    }
}
