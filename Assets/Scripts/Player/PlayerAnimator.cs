using MyTimer;
using Services;
using System.Linq;
using UnityEngine;

public enum EPlayerMotion
{
    /// <summary>
    /// 闲置动作
    /// </summary>
    Idle,
    /// <summary>
    /// 走路动作
    /// </summary>
    Walk,
    /// <summary>
    /// 闲置释放影子动作
    /// </summary>
    SetShadow_Idle,
    /// <summary>
    /// 走路释放影子动作
    /// </summary>
    SetShadow_Walk,
    /// <summary>
    /// 死亡动作
    /// </summary>
    Die,
}

public class PlayerAnimator : CharacterAnimator
{
    private Player player;
    private GameManager gameManager;

    private Bijection<EPlayerMotion, int> MotionAndId;
    private int id_speed;
    private PercentTimer setShadowTimer;

    private bool IsPlayerWalking => player.CurrentSpeed > 1f;

    public EPlayerMotion CurrentMotion => MotionAndId[animator.GetCurrentAnimatorStateInfo(0).tagHash];

    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<Player>();

        MotionAndId = new Bijection<EPlayerMotion, int>();
        System.Type type = typeof(EPlayerMotion);
        foreach (EPlayerMotion value in System.Enum.GetValues(type))
        {
            int hashId = Animator.StringToHash(System.Enum.GetName(type, value));
            MotionAndId.Add(value, hashId);
        }

        id_speed = Animator.StringToHash("Speed");

        setShadowTimer = new PercentTimer();
        setShadowTimer.OnTick += 
            x => PlayMotion(IsPlayerWalking ? EPlayerMotion.SetShadow_Walk : EPlayerMotion.SetShadow_Idle, x);
        setShadowTimer.OnComplete += () =>
        {
            if (IsPlayerWalking) PlayMotion(EPlayerMotion.Walk, 26f / 37f);
            else PlayMotion(EPlayerMotion.Idle, 0f);
        };
        setShadowTimer.Initialize(animator.runtimeAnimatorController.animationClips.First(
            x => x.name.Equals("SetShadow_Idle")).length, false);
    }

    private void Start()
    {
        gameManager = ServiceLocator.Get<GameManager>();
    }

    private void Update()
    {
        animator.SetFloat(id_speed, player.CurrentSpeed);
    }

    public void PlaySetShadowMotion()
    {
        setShadowTimer.Restart();
    }

    public void PlayMotion(EPlayerMotion motion)
    {
        animator.Play(MotionAndId[motion]);
    }

    public void PlayMotion(EPlayerMotion motion, float normalizedTime)
    {
        animator.Play(MotionAndId[motion], 0, normalizedTime);
    }

    public void AfterDie() => gameManager.OpenDieUI();

    public override void ResetAnimator()
    {
        base.ResetAnimator();
        setShadowTimer.Paused = true;
        PlayMotion(EPlayerMotion.Idle);
    }
}
