using MyTimer;
using UnityEngine;

public class FireTrigger : InstantaneousTrigger
{
    private FireInfo fireInfo;
    private Collider2D cd;
    private CountdownTimer damageTrigger;

    protected override void Awake()
    {
        base.Awake();
        cd = GetComponent<Collider2D>();
        destroyTimer.Initialize(0.8f, false);

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

    public void Initialize(FireInfo fireInfo, float burnRadius)
    {
        this.fireInfo = fireInfo;
        SetRadius(burnRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyStatusInfo enemyStatusInfo = collision.GetComponentInParent<EnemyStatusInfo>();
        if (enemyStatusInfo != null && enemyStatusInfo.IsAlive)
        {
            enemyStatusInfo.GetFired(fireInfo);
        }
    }
}
