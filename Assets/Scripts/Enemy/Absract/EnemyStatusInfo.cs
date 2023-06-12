using System.Collections.Generic;
using MyTimer;
using Services;
using UnityEngine;

public class EnemyStatusInfo : CharacterStatusInfo
{
    protected Enemy enemy;
    protected GameObject flameIcon;
    protected EventSystem eventSystem;
    protected AudioManager audioManager;

    protected int dropEXP;
    protected float fireDamage;
    protected FixedDurationRepeation fireTimer;

    public bool IsBoss { get; protected set; }

    protected override void Awake()
    {
        base.Awake();

        enemy = GetComponent<Enemy>();
        // ��ʱд��
        flameIcon = transform.Find("Flame").gameObject;
        eventSystem = ServiceLocator.Get<EventSystem>();
        audioManager=ServiceLocator.Get<AudioManager>();

        fireTimer = new FixedDurationRepeation();
        fireTimer.OnJudge += GetFireHurt;
        fireTimer.OnResume += _ => flameIcon.SetActive(true);
        fireTimer.OnPause += _ => flameIcon.SetActive(false);
        flameIcon.SetActive(false);
        OnGetHurt += GenerateBloodSpatter;
    }

    public void Initialize(float maxHP, bool isBoss, int dropEXP)
    {
        base.Initialize(maxHP);
        IsBoss = isBoss;
        this.dropEXP = dropEXP;
        // this.dropEXP = 50;
    }

    public override void GetHurt(float damage)
    {
        if (IsAlive)
        {
            base.GetHurt(damage);
            objectManager
                .Activate(EObject.DamageText, transform.position)
                .Transform.GetComponent<ShowText>()
                .Init(damage.ToString("0."));
            if(IsAlive)
                audioManager.PlaySound("EnemyHit",AudioPlayMode.Plenty);
        }
    }

    public void GetFired(FireInfo fireInfo)
    {
        fireDamage = fireInfo.damage + MaxHP * fireInfo.HPDamageMultiplier;
        fireTimer.Initialize(fireInfo.fireDuration, fireInfo.judgeInterval);
    }

    protected void GetFireHurt()
    {
        GetHurt(fireDamage);
    }

    protected void GenerateExpParticle()
    {
        List<Vector2> generateCoordinates = ExpRandomer.GenerateCoordinates(
            dropEXP,
            transform.position,
            .45f
        );
        for (int i = 0; i < dropEXP; i++)
        {
            objectManager
                .Activate(EObject.EXP_Particle, transform.position)
                .Transform.GetComponent<EXPParticle>()
                .Init(generateCoordinates[i]);
        }
    }

    protected void GenerateBloodSpatter()
    {
        objectManager.Activate(EObject.BloodSpatter, transform.position, transform);
    }

    public override void Die()
    {
        base.Die();
        audioManager.PlaySound("EnemyDie",AudioPlayMode.Plenty);
        eventSystem.Invoke(EEvent.OnKillEnemy);
        GenerateExpParticle();
        BloodSpatter bloodSpatter = GetComponentInChildren<BloodSpatter>();
        if (bloodSpatter != null)
            bloodSpatter.transform.SetParent(null);
        enemy.DestroySelf();
    }

    public override void ResetStatusInfo()
    {
        base.ResetStatusInfo();
        fireTimer.Paused = true;
    }
}
