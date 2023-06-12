using MyTimer;
using UnityEngine;

public class ExplodeTrigger : InstantaneousTrigger, IDamager
{
    protected Collider2D cd;
    private CountdownTimer damageTrigger;

    protected override void Awake()
    {
        base.Awake();
        cd = GetComponent<Collider2D>();
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

    public float Damage { get; private set; }

    public void Initialize(float damage, float damageRadius)
    {
        Damage = damage;
        SetRadius(damageRadius);
    }
}
