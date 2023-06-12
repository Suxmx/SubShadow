using Services;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatusInfo : CharacterStatusInfo
{
    private GameManager gameManager;
    private AudioManager audioManager;

    public event UnityAction BeforeGetHurt;
    public event UnityAction<DelayDamage> OnGetDelayedHurt;
    public event UnityAction OnRemoveAllDelayedHurt;
    public event UnityAction<float> OnChangeEXP;
    public event UnityAction<int, int> OnChangePlayerGrade;
    public event UnityAction<float, float> OnChangeUnstableHP;

    private DelayDamageManager delayDamageManager;

    [Label("�˺��ӳٿ���")]
    public bool damageDelay;

    [Label("һ���˺�����")]
    public bool invincibleOnce;

    [Label("�����ֵ")]
    public float maxEXP;

    // ��Ϊ����ֵֻ�����Ӳ������
    [SerializeField, Label("����ֵ")]
    private float exp;
    public float EXP
    {
        get => exp;
        set
        {
            exp = value;
            while (exp >= maxEXP)
            {
                exp -= maxEXP;
                PlayerGrade++;
                // ����������
                gameManager.OpenUpgradeUI();
                //����������Ч
                audioManager.PlaySound("LevelUp");
            }
            OnChangeEXP?.Invoke(exp);
        }
    }

    [SerializeField, Label("��ҵȼ�")]
    private int playerGrade;
    public int PlayerGrade
    {
        get => playerGrade;
        set
        {
            // ����maxEXP
            maxEXP = Mathf.Min(Mathf.Floor(0.2f * value * (value + 20f) + 4f + Mathf.Pow(2f, value - 22f)), 10000f);

            OnChangePlayerGrade?.Invoke(playerGrade, value);
            playerGrade = value;
        }
    }

    [SerializeField, Label("���ȶ�����ֵ")]
    private float unstableHP;
    public float UnstableHP
    {
        get => unstableHP;
        set
        {
            OnChangeUnstableHP?.Invoke(unstableHP, value);
            unstableHP = value;
        }
    }

    public override bool IsAlive => CurrentHP > 0 || UnstableHP > 0;

    protected override void Start()
    {
        base.Start();
        gameManager = ServiceLocator.Get<GameManager>();
        audioManager = ServiceLocator.Get<AudioManager>();
        
        delayDamageManager = new DelayDamageManager(this);
        OnGetHurt += GenerateBloodSpatter;
    }

    public void Initialize(SkillInfoData skillInfoData)
    {
        base.Initialize(skillInfoData.playerMaxHP);
        damageDelay = false;
        invincibleOnce = false;
        UnstableHP = 0f;
        EXP = 0f;
        PlayerGrade = 0;
    }

    public void ChangeExp(int exp)
    {
        EXP += exp;
    }

    public override void GetHurt(float damage)
    {
        BeforeGetHurt?.Invoke();
        if (invincibleOnce)
        {
            invincibleOnce = false;
            return;
        }
        if (damageDelay)
        {
            DelayDamage delayDamage = delayDamageManager.AddDelayDamage(damage);
            OnGetDelayedHurt?.Invoke(delayDamage);
        }
        else
            GetRealHurt(damage);
    }

    public void GetRealHurt(float damage)
    {
        audioManager.PlaySound("PlayerHurt");
        if (UnstableHP > 0)
        {
            float decreaseDamage = Mathf.Min(damage, UnstableHP);
            damage -= decreaseDamage;
            UnstableHP -= decreaseDamage;
        }
        base.GetHurt(Mathf.Round(damage));
    }

    public void ClearAllDelayDamages()
    {
        delayDamageManager.ClearAllDelayDamages();
        OnRemoveAllDelayedHurt?.Invoke();
    }

    public void ConvertMaxHPToUnstable(float convertHP = 1f)
    {
        if (MaxHP > 0)
        {
            convertHP = Mathf.Min(convertHP, MaxHP);
            MaxHP -= convertHP;
            UnstableHP += convertHP;
        }
    }

    public void ConvertUnstableHPToMax(float convertHP = 1f)
    {
        MaxHP += convertHP;
        if (UnstableHP > 0)
        {
            convertHP = Mathf.Min(convertHP, UnstableHP);
            UnstableHP -= convertHP;
            CurrentHP += convertHP;
        }
    }

    protected void GenerateBloodSpatter()
    {
        objectManager.Activate(EObject.BloodSpatter_Red, transform.position, transform);
    }

    public override void ResetStatusInfo()
    {
        base.ResetStatusInfo();
        ClearAllDelayDamages();
    }

    public void ResetHP() => base.Initialize(MaxHP);
}
