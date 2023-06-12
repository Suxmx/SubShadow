using MyTimer;
using Services;
using UnityEngine;

public class EnemyAnimator : CharacterAnimator
{
    protected MyObject myObject;
    protected SpriteRenderer sr;
    protected EaseFloatTimer fadeTimer;
    protected AudioManager audioManager;

    // ´ýÍêÉÆ
    protected bool hasDestroySelfMotion;

    protected override void Awake()
    {
        base.Awake();
        myObject = GetComponentInParent<MyObject>();
        sr = GetComponent<SpriteRenderer>();
        audioManager=ServiceLocator.Get<AudioManager>();

        hasDestroySelfMotion = false;
        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("DestroySelf"))
            {
                hasDestroySelfMotion = true;
                fadeTimer = new EaseFloatTimer(EaseType.InCubic);
                fadeTimer.OnTick += SetAnimatorAlpha;
                fadeTimer.Initialize(1f, 0f, clip.length, false);
                myObject.OnActivate += () => SetAnimatorAlpha(1f);
                myObject.OnRecycle += () => fadeTimer.Paused = true;
                break;
            }
        }
    }

    public virtual void PlayDestroySelfMotion()
    {
        ResetAnimator();
        UnlockAnimator();
        if (hasDestroySelfMotion)
        {
            animator.Play("DestroySelf");
            fadeTimer.Restart();
        }
        else Disappear();
    }

    protected void SetAnimatorAlpha(float x)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, x);
    }

    protected virtual void Disappear() => myObject.Recycle();
}
