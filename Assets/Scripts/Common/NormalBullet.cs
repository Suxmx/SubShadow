using MyTimer;
using UnityEngine;

public class NormalBullet : Bullet, IDestroySelf
{
    protected Animator animator;
    protected Collider2D cd;
    protected SpriteRenderer sr;
    protected EaseFloatTimer fadeTimer;
    protected Vector3 defaultScale;

    // ´ýÍêÉÆ
    protected bool hasDestroySelfMotion;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        cd = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultScale = transform.localScale;

        hasDestroySelfMotion = false;
        if (animator != null)
        {
            foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("DestroySelf"))
                {
                    hasDestroySelfMotion = true;
                    fadeTimer = new EaseFloatTimer(EaseType.InCubic);
                    fadeTimer.Initialize(1f, 0f, clip.length, false);
                    fadeTimer.OnTick += SetAnimatorAlpha;
                    fadeTimer.OnComplete += myObject.Recycle;
                    myObject.OnActivate += () => SetAnimatorAlpha(1f);
                    break;
                }
            }
        }
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        transform.localScale = defaultScale;
        if (hasDestroySelfMotion)
        {
            animator.Play("Move");
            cd.enabled = true;
        }
    }

    protected void SetAnimatorAlpha(float x)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, x);
    }

    protected override void OnDestroySelf()
    {
        if (hasDestroySelfMotion)
        {
            animator.Play("DestroySelf");
            cd.enabled = false;
            fadeTimer.Restart();
        }
        else myObject.Recycle();
    }
}
