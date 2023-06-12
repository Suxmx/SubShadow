using MyTimer;
using Services;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 人物状态信息的基类
/// </summary>
public class CharacterStatusInfo : MonoBehaviour
{
    protected ObjectManager objectManager;
    public CountdownTimer flickerTimer;

    public event UnityAction<float, float> OnChangeMaxHP;
    public event UnityAction<float, float> OnChangeHP;
    public event UnityAction OnGetHurt;
    public event UnityAction OnGetHealed;
    public event UnityAction OnDie;

    [SerializeField, Label("最大生命值")]
    protected float maxHP;
    public virtual float MaxHP
    {
        get => maxHP;
        set
        {       
            OnChangeMaxHP?.Invoke(maxHP, value);
            maxHP = value;
            if (CurrentHP > maxHP)
                CurrentHP = maxHP;
        }
    }

    [SerializeField, Label("当前生命值")]
    protected float currentHP;
    public virtual float CurrentHP
    {
        get => currentHP;
        set
        {
            OnChangeHP?.Invoke(currentHP, value);
            currentHP = value;
        }
    }

    public virtual bool IsAlive => CurrentHP > 0;

    protected virtual void Awake()
    {
        flickerTimer = new CountdownTimer();
        maxHP = currentHP = 0;
        CharacterAnimator animator = GetComponentInChildren<CharacterAnimator>();
        Material material = animator.GetComponent<Renderer>().material;
        flickerTimer.OnResume += _ =>
        {
            animator.AddAnimatorLock();
            material.SetInt("_TurnColor", 1);
        };
        flickerTimer.OnPause += _ =>
        {
            animator.RemoveAnimatorLock();
            material.SetInt("_TurnColor", 0);
        };
        flickerTimer.Initialize(0.2f, false);

        OnGetHurt += () => flickerTimer.Restart();
    }

    protected virtual void Start()
    {
        objectManager = ServiceLocator.Get<ObjectManager>();
    }

    public virtual void Initialize(float maxHP)
    {
        
        MaxHP = maxHP;
        CurrentHP = MaxHP;
    }

    public virtual void GetHurt(float damage)
    {
        CurrentHP = Mathf.Max(CurrentHP - damage, 0);
        OnGetHurt?.Invoke();
        if (!IsAlive)
            Die();
    }

    public virtual void Die()
    {
        OnDie?.Invoke();
    }

    public virtual void GetHealed(float heal)
    {
        CurrentHP = Mathf.Min(CurrentHP + heal, MaxHP);
        OnGetHealed?.Invoke();
    }

    public virtual void ResetStatusInfo()
    {
        flickerTimer.Paused = true;
    }
}
